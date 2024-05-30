using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using TicketsRavelli.Application.Services.Interfaces;
using TicketsRavelli.Application.InputModels.Inscricao;
using Newtonsoft.Json;
using TicketsRavelli.Controllers.Inscricao;

namespace TicketsRavelli.API.Controllers.Inscricao;

[ApiController]
[Route("v1/[controller]")]
public class SubscriptionController : ControllerBase
{
    private readonly ISubscriptionService _subscriptionService;
    private readonly ILogSystem _logger;

    public SubscriptionController(ISubscriptionService subscriptionService, ILogSystem logger)
    {
        _subscriptionService = subscriptionService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = "EmployeePolicy")]
    public async Task<IActionResult> GetAll()
    {
        var subscriptions = await _subscriptionService.GetAllAsync();

        if (!subscriptions.Any())
            return NotFound();

        return Ok(subscriptions);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<IActionResult> GetById(int id)
    {
        var subscription = await _subscriptionService.GetByIdAsync(id);

        if (subscription == null)
            return NotFound();

        return Ok(subscription);
    }

    [HttpGet("event/{idEvent}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AllowAnonymous]
    public async Task<IActionResult> GetByEvent(int idEvent)
    {
        var subscriptions = await _subscriptionService.GetByEventAsync(idEvent);

        if (!subscriptions.Any())
            return NotFound();

        return Ok(subscriptions);
    }

    [HttpGet("athlete-subscribed/{cpfAthlete}/{idEvent}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AllowAnonymous]
    public async Task<IActionResult> CheckIfAthleteSubscribedByEvent(string cpfAthlete, int idEvent)
    {
        var athleteSubscribed = await _subscriptionService.CheckIfAthleteSubscribedByEventAsync(cpfAthlete, idEvent);

        return athleteSubscribed ? Ok() : NotFound();
    }

    [HttpGet("no-effectives/{idEvent}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = "EmployeePolicy")]
    public async Task<IActionResult> GetNonEffectiveSubscriptionsByEvent(int idEvent)
    {
        var subscriptons = await _subscriptionService.GetNonEffectiveSubscriptionsByEventAsync(idEvent);

        if (!subscriptons.Any())
            return NotFound();

        return Ok(subscriptons);
    }

    [HttpGet("athlete/{cpfAthlete}")]
    [Authorize]
    public async Task<IActionResult> GetAthleteSubscriptions(string cpfAthlete)
    {
        var subscriptions = await _subscriptionService.GetAthleteSubscriptionsViewModel(cpfAthlete);

        if (!subscriptions.Any())
        {
            return NotFound(new { message = "Nenhuma inscrição encontrada" });
        }

        return Ok(subscriptions);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize]
    public async Task<IActionResult> Post(SubscriptionInputModel subscriptionInputModel)
    {
        try
        {
            int idSubscription = await _subscriptionService.CreateAsync(subscriptionInputModel);

            return Created($"inscricao/{idSubscription}", idSubscription);
        }
        catch (Exception ex)
        {
            _logger.SaveLog(ex.Message);
            _logger.SaveLog("Inscricao: " + JsonConvert.SerializeObject(subscriptionInputModel));

            return BadRequest();
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<IActionResult> Put(int id, SubscriptionInputModel subscriptionInputModel)
    {
        var subscription = await _subscriptionService.GetByIdAsync(id);

        if (subscription == null)
            return NotFound();

        await _subscriptionService.UpdateAsync(subscription, subscriptionInputModel);

        return NoContent();
    }

    [HttpPut("effect/{idSubscription}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Policy = "EmployeePolicy")]
    public async Task<IActionResult> Submit(int idSubscription, EffectSubscriptionInputModel effectSubscriptionInputModel)
    {
        var inscricao = await _subscriptionService.GetByIdAsync(idSubscription);

        if (inscricao == null)
            return NotFound();

        try
        {
            await _subscriptionService.SubmitAsync(inscricao,
                Decimal.Parse(effectSubscriptionInputModel.ValorPago),
                effectSubscriptionInputModel.MetodoPagamento);

            return NoContent();
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = "EmployeePolicy")]
    public async Task<IActionResult> Delete(int id)
    {
        var inscricao = await _subscriptionService.GetByIdAsync(id);

        if (inscricao == null)
            return NotFound();

        await _subscriptionService.DeleteAsync(inscricao);

        return Ok();
    }
}