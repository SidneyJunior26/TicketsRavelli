using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketsRavelli.Application.InputModels.Pix;
using TicketsRavelli.Application.Services.Interfaces;
using TicketsRavelli.Infrastructure.GerenciaNet.Interfaces;

namespace TicketsRavelli.API.Controllers.Pagamentos.Pix;

[ApiController]
[Route("v1/[controller]")]
public class PixController : ControllerBase
{
    private readonly IPixService _pixService;
    private readonly ISubscriptionService _inscricaoService;
    private readonly IEventService _eventoService;
    private readonly IDiscountService _descontoService;

    public PixController(IPixService pixService, ISubscriptionService inscricaoService, IEventService eventoService, IDiscountService descontoService)
    {
        _pixService = pixService;
        _inscricaoService = inscricaoService;
        _eventoService = eventoService;
        _descontoService = descontoService;
    }

    [HttpGet("{idInscricao}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<IActionResult> GetStatus(int idInscricao)
    {
        var paid = false;
        var subscription = await _inscricaoService.GetByIdAsync(idInscricao);

        if (subscription == null)
            return NotFound();

        var pix = _pixService.GetStatus(subscription.GnChargeTxId);

        if (pix["status"] == null)
            return BadRequest();

        await _inscricaoService.UpdatePaymentStatusAsync(subscription, pix["status"].ToString());

        if (pix["status"].ToString() == "CONCLUIDA")
        {
            var paidValue = Convert.ToDecimal(pix["valor"]["original"].ToString());

            decimal valueToSave = Convert.ToDecimal(paidValue) / Convert.ToDecimal(100.0);

            await _inscricaoService.ConfirmPaymentAsync(subscription, Convert.ToDecimal(valueToSave.ToString("#.00")));

            paid = true;
        }

        return Ok(new { pago = paid });
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<IActionResult> GeneratePix(PixInputModel pixInputModel)
    {
        var subscription = await _inscricaoService.GetByIdAsync(pixInputModel.IdInscricao);

        if (subscription == null)
            return NotFound("Inscrição não localizada");

        var packageValue = await _eventoService.GetPackageValueAsync(subscription.Pacote, subscription.IdEvento);

        if (pixInputModel.CupomDesconto != "")
        {
            var coupom = await _descontoService.ValidateAsync(pixInputModel.CupomDesconto, subscription.IdEvento);

            if (coupom != null && coupom.PorcDesconto > 0)
            {
                packageValue = _descontoService.GetFinalValue(coupom, packageValue);
            }
        }

        var transaction = _pixService.CreateImmediateBilling(subscription.CpfAtleta, subscription.Atleta.Nome, packageValue, subscription.Evento.Nome);

        if (transaction["mensagem"] != null)
            return BadRequest(new { errorMessage = transaction["mensagem"].ToString() });

        if (transaction["txid"] == null)
            return BadRequest("Erro ao gerar Pix");

        await _inscricaoService.UpdateTxIdAsync(subscription, transaction["txid"].ToString(), transaction["status"].ToString());

        var qrCodeJson = _pixService.GenerateQrCode(transaction["loc"]["id"].ToString());

        return Ok(new { qrCode = qrCodeJson["qrcode"].ToString(), qrCodeImg = qrCodeJson["imagemQrcode"].ToString() });
    }

    [HttpGet("confirmar-pagamentos/{idEvento}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Policy = "EmployeePolicy")]
    public async Task<IActionResult> ConfirmarPagamentos(int idEvento)
    {
        int qtdCompleted = 0;
        var subscriptionsNotPaid = await _inscricaoService.GetUnpaidPixSubscriptionsAsync(idEvento);

        if (!subscriptionsNotPaid.Any())
            return NotFound();

        foreach (var subscription in subscriptionsNotPaid)
        {
            var pix = _pixService.GetStatus(subscription.GnChargeTxId);

            if (pix["status"] == null)
                return BadRequest();

            await _inscricaoService.UpdatePaymentStatusAsync(subscription, pix["status"].ToString());

            if (pix["status"].ToString() == "CONCLUIDA")
            {
                var paidValue = Convert.ToDecimal(pix["valor"]["original"].ToString());

                decimal valueToSave = Convert.ToDecimal(paidValue) / Convert.ToDecimal(100.0);

                await _inscricaoService.ConfirmPaymentAsync(subscription, Convert.ToDecimal(valueToSave.ToString("#.00")));

                qtdCompleted++;
            }
        }

        return Ok(new { message = "Quantidade de Pix Concluídos: " + qtdCompleted });
    }
}