using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketsRavelli.Infrastructure.GerenciaNet.Interfaces;
using TicketsRavelli.Infrastructure.InputModels;

namespace TicketsRavelli.API.Controllers.Pagamentos.Cartão;

public class CartãoController : ControllerBase {
    private readonly ITransacaoService _transacaoService;

    public CartãoController(ITransacaoService transacaoService) {
        _transacaoService = transacaoService;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CriarTransacao(TransacaoInputModel transacaoInputModel) {
        var transacao = _transacaoService.CreateTransaction(transacaoInputModel);

        return Ok();
    }
}

