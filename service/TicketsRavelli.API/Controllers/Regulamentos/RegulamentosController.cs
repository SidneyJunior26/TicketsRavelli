using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketsRavelli.Application.Services.Interfaces;

namespace TicketsRavelli.Controllers.Regulamentos;

[ApiController]
[Route("v1/[controller]")]
public class RegulamentosController : ControllerBase {
    private readonly IRegulationService _regulationService;

    public RegulamentosController(IRegulationService regulationService) {
        _regulationService = regulationService;
    }

    [HttpGet("{idEvento}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AllowAnonymous]
    public async Task<IActionResult> GetByEvent(int idEvento) {
        var regulations = await _regulationService.GetByEventAsync(idEvento);

        if (regulations == null)
            return NotFound();

        return Ok(regulations);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [Authorize(Policy = "EmployeePolicy")]
    public async Task<IActionResult> CadastrarRegulamento(NewRegulationInputModel novoRegulamentoInputModel) {
        await _regulationService.CreateAsync(novoRegulamentoInputModel);

        return Created("regulamentos/" + novoRegulamentoInputModel.idEvento, novoRegulamentoInputModel);
    }

    [HttpPut("{idEvento}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = "EmployeePolicy")]
    public async Task<IActionResult> Put(int idEvento, UpdateRegulationInputModel editarRegulamentoInputModel) {
        var regulation = await _regulationService.GetByEventAsync(idEvento);

        if (regulation == null)
            return NotFound();

        await _regulationService.UpdateAsync(regulation, editarRegulamentoInputModel);

        return NoContent();
    }
}