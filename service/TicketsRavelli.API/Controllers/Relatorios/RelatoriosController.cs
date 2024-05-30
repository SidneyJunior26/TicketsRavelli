using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketsRavelli.Application.Services.Interfaces;

namespace TicketsRavelli.API.Controllers.Relatorios;

[ApiController]
[Route("v1/[controller]")]
public class RelatoriosController : ControllerBase
{
    private readonly IReportService _reportService;
    private readonly IEventService _eventService;

    public RelatoriosController(IReportService reportService, IEventService eventService)
    {
        _reportService = reportService;
        _eventService = eventService;
    }

    [HttpGet("{idEvento}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = "EmployeePolicy")]
    public async Task<IActionResult> SubscriptionsReport(int idEvento)
    {
        var eventById = await _eventService.GetByIdAsync(idEvento);

        if (eventById == null)
            return NotFound(new { message = "Evento não encontrado" });

        var report = await _reportService.GetSubscriptionsReportAsync(idEvento);

        if (report == null)
            return NotFound();

        return Ok(report);
    }

    [HttpGet("inscritos-categoria/{idEvento}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = "EmployeePolicy")]
    public async Task<IActionResult> GetReportSubscriptionsCategories(int idEvento)
    {
        var eventById = await _eventService.GetByIdAsync(idEvento);

        if (eventById == null)
            return NotFound(new { message = "Evento não encontrado" });

        var report = await _reportService.GetSubscriptionsCategoriesReportAsync(idEvento);

        if (report == null)
            return NotFound();

        return Ok(report);
    }

    [HttpGet("total-inscritos-categoria/{idEvento}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = "EmployeePolicy")]
    public async Task<IActionResult> GetTotalSubscriptionsByCategories(int idEvento)
    {
        var eventById = await _eventService.GetByIdAsync(idEvento);

        if (eventById == null)
            return NotFound(new { message = "Evento não encontrado" });

        var report = await _reportService.GetTotalSubscriptionsByCategoriesReportAsync(idEvento);

        if (report == null)
            return NotFound();

        return Ok(report);
    }

    [HttpGet("total-efetivados/{idEvento}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = "EmployeePolicy")]
    public async Task<IActionResult> GetTotalEffectiveSubscriptions(int idEvento)
    {
        var eventById = await _eventService.GetByIdAsync(idEvento);

        if (eventById == null)
            return NotFound(new { message = "Evento não encontrado" });

        var report = await _reportService.GetTotalEffectiveSubscriptionsAsync(idEvento);

        if (report == null)
            return NotFound();

        return Ok(report);
    }

    [HttpGet("total-camisetas/{idEvento}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = "EmployeePolicy")]
    public async Task<IActionResult> GetTotalShirts(int idEvento)
    {
        var eventById = await _eventService.GetByIdAsync(idEvento);

        if (eventById == null)
            return NotFound(new { message = "Evento não encontrado" });

        var report = await _reportService.GetTotalShirtsCategoriesAsync(idEvento);

        if (report == null)
            return NotFound();

        return Ok(report);
    }
}