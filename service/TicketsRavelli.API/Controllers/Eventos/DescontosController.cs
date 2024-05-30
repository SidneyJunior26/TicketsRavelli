using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketsRavelli.Application.InputModels.Eventos;
using TicketsRavelli.Application.Services.Interfaces;

namespace TicketsRavelli.API.Controllers.Eventos;

[ApiController]
[Route("v1/[controller]")]
public class DescontosController : ControllerBase
{
    private readonly IDiscountService _descountService;
    private readonly IEventService _eventService;

    public DescontosController(IDiscountService descontoService, IEventService eventService)
    {
        _descountService = descontoService;
        _eventService = eventService;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = "EmployeePolicy")]
    public async Task<IActionResult> GetById(int id)
    {
        var coupom = await _descountService.GetByIdAsync(id);

        if (coupom == null)
            return NotFound(new { message = "Cupom não localizado" });

        return Ok(coupom);
    }

    [HttpGet("evento/{idEvento}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = "EmployeePolicy")]
    public async Task<IActionResult> GetByEvent(int idEvento)
    {
        var coupons = await _descountService.GetByEventAsync(idEvento);

        if (coupons == null)
            return NotFound(new { message = "Nenhum cupom para este evento" });

        return Ok(coupons);
    }

    [HttpGet("validar/{cupom}/{idEvento}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AllowAnonymous]
    public async Task<IActionResult> ValidateCoupom(string cupom, int idEvento)
    {
        var coupom = await _descountService.ValidateAsync(cupom, idEvento);

        if (coupom == null)
            return NotFound(new { message = "Este cupom não é válido" });

        return Ok(new { porcentagem = coupom.PorcDesconto });
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = "EmployeePolicy")]
    public async Task<IActionResult> Create(NewCoupomInputModel newCoupomInputModel)
    {
        var eventById = await _eventService.GetByIdAsync(newCoupomInputModel.idEvento);

        if (eventById == null)
            return NotFound(new { message = "Evento não encontrado." });

        var idDesconto = await _descountService.CreateAsync(newCoupomInputModel);

        return Created($"descontos/{idDesconto}", newCoupomInputModel);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = "EmployeePolicy")]
    public async Task<IActionResult> Update(int id, UpdateCoupomInputModel updateCoupomInputModel)
    {
        var coupom = await _descountService.GetByIdAsync(id);

        if (coupom == null)
            return NotFound(new { message = "Cupom de desconto não localizado." });

        await _descountService.Update(coupom, updateCoupomInputModel);

        return NoContent();
    }

    [HttpPut("desativar/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = "EmployeePolicy")]
    public async Task<IActionResult> Disable(int id)
    {
        var coupom = await _descountService.GetByIdAsync(id);

        if (coupom == null)
            return NotFound(new { message = "Cupom de desconto não localizado." });

        await _descountService.DisableAsync(coupom);

        return NoContent();
    }

    [HttpPut("ativar/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = "EmployeePolicy")]
    public async Task<IActionResult> Activate(int id)
    {
        var desconto = await _descountService.GetByIdAsync(id);

        if (desconto == null)
            return NotFound(new { message = "Cupom de desconto não localizado." });

        await _descountService.ActivateAsync(desconto);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = "EmployeePolicy")]
    public async Task<IActionResult> Delete(int id)
    {
        var coupom = await _descountService.GetByIdAsync(id);

        if (coupom == null)
            return NotFound();

        await _descountService.DeleteAsync(coupom);

        return NoContent();
    }
}