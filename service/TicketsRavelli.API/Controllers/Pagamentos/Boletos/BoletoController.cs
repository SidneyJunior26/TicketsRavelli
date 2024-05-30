using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TicketsRavelli.Application.InputModels.Inscricao;
using TicketsRavelli.Application.Services.Interfaces;
using TicketsRavelli.Infrastructure.GerenciaNet.Interfaces;
using TicketsRavelli.Infrastructure.InputModels;

namespace TicketsRavelli.API.Controllers.Pagamentos.Boletos;

[ApiController]
[Route("v1/[controller]")]
public class BoletoController : ControllerBase
{
    private readonly ITransacaoService _transacaoService;
    private readonly IBoletoService _boletoService;
    private readonly ISubscriptionService _inscricaoService;
    private readonly IEventService _eventoService;
    private readonly IDiscountService _descontoService;
    private readonly ILogSystem _logger;

    public BoletoController(ITransacaoService transacaoService, IBoletoService boletoService, ISubscriptionService inscricaoService,
        ILogSystem logger, IEventService eventoService, IDiscountService descontoService)
    {
        _boletoService = boletoService;
        _inscricaoService = inscricaoService;
        _transacaoService = transacaoService;
        _logger = logger;
        _eventoService = eventoService;
        _descontoService = descontoService;
    }

    [HttpPost("{idInscricao}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize]
    public async Task<IActionResult> GenerateBillet(int idInscricao, TransacaoInputModel billetInputModel)
    {
        try
        {
            var subscription = await _inscricaoService.GetByIdAsync(idInscricao);

            if (subscription == null)
                return NotFound(new { mensagem = "Ocorreu um erro ao localizar a inscricão." });

            var packageValue = await _eventoService.GetPackageValueAsync(subscription.Pacote, subscription.IdEvento);

            if ((billetInputModel.CupomDesconto != "") || (billetInputModel.Valor.Length < 4 || billetInputModel.Valor.Length > 5))
            {
                var cupomDesconto = await _descontoService.ValidateAsync(billetInputModel.CupomDesconto, subscription.IdEvento);

                if (cupomDesconto != null && cupomDesconto.PorcDesconto > 0)
                {
                    packageValue = _descontoService.GetFinalValue(cupomDesconto, packageValue);
                }

                billetInputModel.UpdateValue(packageValue.ToString());
            }

            var transaction = _transacaoService.CreateTransaction(billetInputModel);

            if (transaction == null)
                return BadRequest(new { mensagem = "Ocorreu um erro para gerar a transação." });

            var idTransacao = transaction["data"]["charge_id"].ToString();

            var billet = _boletoService.Generate(idTransacao, billetInputModel, subscription.Evento);

            if (billet == null)
                return BadRequest(new { mensagem = "Ocorreu um erro para gerar o boleto." });

            if (billet["error_description"] != null && billet["error_description"].ToString() != "")
            {
                return BadRequest(new { errorMessage = billet["error_description"].ToString() });
            }

            SaveBilletInputModel salvarBoletoInscricaoInput = new SaveBilletInputModel(
                    DateTime.Parse(billet["data"]["expire_at"].ToString()),
                    int.Parse(billet["data"]["charge_id"].ToString()),
                    int.Parse(billet["data"]["total"].ToString()),
                    billet["data"]["link"].ToString(),
                    billet["data"]["barcode"].ToString(),
                    billet["data"]["status"].ToString()
                    );

            await _inscricaoService.SaveBilletAsync(subscription, salvarBoletoInscricaoInput);

            return Ok(new { link = billet["data"]["link"].ToString() });
        }
        catch (Exception ex)
        {
            _logger.SaveLog(ex.Message);
            _logger.SaveLog("Boleto: " + JsonConvert.SerializeObject(billetInputModel));
            _logger.SaveLog(ex.InnerException.ToString());

            return BadRequest(new { message = ex.Message, inner = ex.InnerException });
        }
    }

    [HttpPost("notificacao")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [AllowAnonymous]
    public async Task<IActionResult> NotificationGn([FromForm] string notification)
    {
        try
        {
            var transaction = _boletoService.GetNotificationGn(notification);

            if (transaction["code"].ToString() != "200")
                return BadRequest("Notificação não localizada.");

            for (int i = 0; i < transaction["data"].Count(); i++)
            {
                var txId = transaction["data"][i]["identifiers"]["charge_id"].ToString();

                var subscription = await _inscricaoService.GetChargeByTxIdAsync(int.Parse(txId));

                if (subscription == null)
                    continue;

                string status = transaction["data"][i]["status"]["current"].ToString();

                await _inscricaoService.UpdatePaymentStatusAsync(subscription, status);

                if (status == "paid")
                {
                    var amountPaid = Convert.ToDecimal(transaction["data"][i]["value"].ToString());

                    decimal valueToSave = Convert.ToDecimal(amountPaid) / Convert.ToDecimal(100.0);

                    await _inscricaoService.ConfirmPaymentAsync(subscription, Convert.ToDecimal(valueToSave.ToString("#.00")));
                }
            }

            return Ok(transaction);
        }
        catch (Exception ex)
        {
            _logger.SaveLog(ex.Message);
            _logger.SaveLog("Erro na notificação do boleto");

            return BadRequest(ex.Message + " " + ex.InnerException);
        }
    }

    [HttpGet("confirmar-pagamentos/{idEvento}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmPayment(int idEvento)
    {
        int qtdCompleted = 0;
        var subscriptionsBilletNotPaids = await _inscricaoService.GetUnpaidBilletSubscriptionsAsync(idEvento);

        if (!subscriptionsBilletNotPaids.Any())
            return NoContent();

        foreach (var subscription in subscriptionsBilletNotPaids)
        {
            try
            {
                var transaction = _boletoService.ConsultarStatusBoletos((int)subscription.GnChargeId);

                var status = transaction["data"]["status"].ToString();

                if (status == "paid")
                {
                    var valorPago = Convert.ToDecimal(transaction["data"]["total"].ToString());

                    decimal valorParaSalvar = Convert.ToDecimal(valorPago) / Convert.ToDecimal(100.0);

                    await _inscricaoService.ConfirmPaymentAsync(subscription, Convert.ToDecimal(valorParaSalvar.ToString("#.00")));

                    qtdCompleted++;
                }
                else if (status == "unpaid")
                {
                    await _inscricaoService.UpdatePaymentStatusAsync(subscription, status);
                }
            }
            catch { }
        }

        return Ok(new { message = "Quantidade de Pix Concluídos: " + qtdCompleted });
    }
}

