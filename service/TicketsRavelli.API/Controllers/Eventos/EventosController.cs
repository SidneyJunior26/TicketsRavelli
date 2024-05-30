using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketsRavelli.Application.Services.Interfaces;
using TicketsRavelli.Application.ViewModels.Eventos;
using TicketsRavelli.Core.Entities.Eventos;

namespace TicketsRavelli.Controllers.Eventos;

[ApiController]
[Route("v1/[controller]")]
public class EventsController : ControllerBase
{
    private readonly IEventService _eventService;
    private readonly ISubscriptionService _subscriptionService;
    private readonly IImagemService _imageService;
    private readonly ILogSystem _logger;

    public EventsController(IEventService eventService,
        IImagemService imageService,
        ISubscriptionService subscriptionService,
        ILogSystem logger)
    {
        _eventService = eventService;
        _subscriptionService = subscriptionService;
        _imageService = imageService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = "EmployeePolicy")]
    public async Task<IActionResult> GetAllEvents()
    {
        List<EventoComEstatisticasInscricao> events;

        try
        {
            events = await _eventService.GetAllWithStatisticsAsync();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        if (!events.Any())
            return NotFound();

        return Ok(events);
    }

    [HttpGet("actives")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AllowAnonymous]
    public async Task<IActionResult> GetAllEventsActives()
    {
        List<Evento> events;

        try
        {
            events = await _eventService.GetAllActivesAsync();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        if (!events.Any())
            return NotFound();

        return Ok(events);
    }

    [HttpGet("upcoming")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AllowAnonymous]
    public async Task<IActionResult> GetUpcomingEvents()
    {
        List<Evento> events;

        try
        {
            events = await _eventService.GetUpComingAsync();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        if (events == null)
            return NotFound();

        return Ok(events);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AllowAnonymous]
    public async Task<IActionResult> GetEventById(int id)
    {
        var eventById = await _eventService.GetByIdAsync(id);

        if (eventById == null)
            return NotFound();

        return Ok(eventById);
    }

    [HttpGet("packages/{idEvent}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AllowAnonymous]
    public async Task<IActionResult> GetPackagesByEvent(int idEvent)
    {
        var packagesEvent = await _eventService.GetPackagesAsync(idEvent);

        if (packagesEvent == null)
            return NotFound();

        return Ok(packagesEvent);
    }

    [HttpGet("package-value/{idSubscription}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AllowAnonymous]
    public async Task<IActionResult> GetPackageValue(int idSubscription)
    {
        var subscription = await _subscriptionService.GetByIdAsync(idSubscription);

        if (subscription == null && subscription.Pacote == null)
            return NotFound();

        if (subscription.Pacote == null)
            return NotFound();

        var packageValue = await _eventService.GetPackageValueAsync(subscription.Pacote, subscription.IdEvento);

        if (packageValue == 0)
            return NotFound();

        return Ok(new { valor = packageValue.ToString().Replace(",", "") });
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = "EmployeePolicy")]
    public async Task<IActionResult> DeleteEvent(int id)
    {
        var eventBydId = await _eventService.GetByIdAsync(id);

        if (eventBydId == null)
            return NotFound();

        await _eventService.DeleteAsync(eventBydId);

        return Ok();
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [Authorize(Policy = "EmployeePolicy")]
    public async Task<IActionResult> PostEvent(EventInputModel eventInputModel)
    {
        int idEvent;

        try
        {
            idEvent = await _eventService.CreateAsync(eventInputModel);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

        return Created($"/events/{idEvent}", idEvent);
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = "EmployeePolicy")]
    public async Task<IActionResult> PutEvent(int id, EventInputModel eventRequest)
    {
        try
        {
            var eventById = await _eventService.GetByIdAsync(id);

            if (eventById == null)
                return NotFound();

            await _eventService.UpdateAsync(eventById, eventRequest);

            return NoContent();

        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("image/{idEvent}")]
    [Authorize(Policy = "EmployeePolicy")]
    public async Task<IActionResult> Post(int idEvent, IFormFile file)
    {
        if (file == null || file.Length == 0)
        {
            return BadRequest("Arquivo inválido");
        }

        var eventById = await _eventService.GetByIdAsync(idEvent);

        if (eventById == null)
        {
            return NotFound("Evento não encontrado");
        }

        try
        {
            using (var memoryStream = new MemoryStream())
            {
                var fileName = idEvent + DateTime.Now.ToString("yyyyMMdd_HHmmss");

                await file.CopyToAsync(memoryStream);
                byte[] imageBytes = memoryStream.ToArray();

                await _imageService.SaveImageEventAsync(fileName, imageBytes);

                await _eventService.UpdateImageAsync(eventById, fileName);

                return Ok();
            }
        }
        catch (Exception ex)
        {
            _logger.SaveLog("Erro ao salvar imagem: " + ex.Message);

            return StatusCode(500, "Erro no upload: " + ex.Message);
        }
    }
}