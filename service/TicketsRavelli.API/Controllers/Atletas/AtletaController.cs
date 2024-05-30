using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TicketsRavelli.Application.InputModels.Atletas;
using TicketsRavelli.Application.Services.Interfaces;
using TicketsRavelli.Core.Entities.Athletes;
using TicketsRavelli.Core.Entities.Eventos;

namespace TicketsRavelli.Controllers.Atletas;

[ApiController]
[Route("v1/[controller]")]
public class AtletasController : ControllerBase {
    private readonly IAthleteService _athleteService;
    private readonly ISubscriptionService _subscriptionService;
    private readonly ILogSystem _logger;

    public AtletasController(IAthleteService atlheteService, ILogSystem logger, ISubscriptionService subscriptionService) {
        _athleteService = atlheteService;
        _logger = logger;
        _subscriptionService = subscriptionService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = "EmployeePolicy")]
    public async Task<IActionResult> GetAllAthletes() {
        List<Athlete> athletes;

        try {
            athletes = await _athleteService.GetAllAsync();

        } catch (Exception ex) {
            return BadRequest(ex.Message);
        }

        if (athletes == null)
            return NotFound(new { message = "Nenhum atleta encontrado" });

        return Ok(athletes);
    }

    [HttpGet("existe/{cpf}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AllowAnonymous]
    public async Task<IActionResult> CheckAthleteExists([FromRoute] string cpf) {
        var athleteExists = await _athleteService.CheckAthleteExistsByCpfAsync(cpf);

        if (!athleteExists)
            return NotFound(new { message = "Usuário não encontrado" });

        return Ok();
    }

    [HttpGet("existe-email/{email}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AllowAnonymous]
    public async Task<IActionResult> CheckAthleteRegisteredByEmail([FromRoute] string email) {
        var athleteExists = await _athleteService.CheckAthleteRegisteredByEmailAsync(email);

        if (!athleteExists)
            return NotFound(new { message = "Usuário não encontrado" });

        return Ok();
    }

    [HttpGet("existe-rg/{rg}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AllowAnonymous]
    public async Task<IActionResult> CheckAthleteRegisteredByRG([FromRoute] string rg) {
        var athleteExists = await _athleteService.CheckAthleteRegisteredByRGAsync(rg);

        if (!athleteExists)
            return NotFound(new { message = "Usuário não encontrado" });

        return Ok();
    }

    [HttpGet("consultar/{cpf}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<IActionResult> GetAthleteByCpf(string cpf) {
        var athlete = await _athleteService.GetByCpfAsync(cpf);

        if (athlete == null)
            return NotFound(new { message = "Usuário não encontrado" });

        return Ok(athlete);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [AllowAnonymous]
    public async Task<IActionResult> PostAthlete(NewAthleteInputModel newAthleteInputModel) {
        var athleteExists = await _athleteService.CheckAthleteExistsByCpfAsync(newAthleteInputModel.Cpf);

        if (athleteExists)
            return NotFound(new { message = "Já existe um usuário cadastrado com este CPF" });

        try {
            var athlete = await _athleteService.CreateAsync<NewAthleteInputModel>(newAthleteInputModel);

            return Ok(new { idAtleta = athlete.Id });
        } catch (Exception ex) {
            _logger.SaveLog("Erro ao cadastrar Atleta");
            _logger.SaveLog("Atleta: " + JsonConvert.SerializeObject(newAthleteInputModel));
            _logger.SaveLog(ex.Message);

            return BadRequest(new { menssage = "Ocorreu um erro ao cadastrar seus dados" });
        }
    }

    [HttpPost("adm")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = "EmployeePolicy")]
    public async Task<IActionResult> PostAthleteAdm(NovoAtletaAdmInputModel atletaInputModel) {
        var athleteExists = await _athleteService.CheckAthleteExistsByCpfAsync(atletaInputModel.Cpf);

        if (athleteExists)
            return NotFound(new { message = "Já existe um usuário cadastrado com este CPF" });

        try {
            var atleta = await _athleteService.CreateAsync<NovoAtletaAdmInputModel>(atletaInputModel);

            return Ok(new { idAtleta = atleta.Id });
        } catch (Exception ex) {
            _logger.SaveLog("Erro ao cadastrar Atleta");
            _logger.SaveLog("Atleta: " + JsonConvert.SerializeObject(atletaInputModel));
            _logger.SaveLog(ex.Message);

            return BadRequest(new { menssage = "Ocorreu um erro ao cadastrar seus dados" });
        }
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<IActionResult> UpdateAthlete([FromRoute] string id, UpdateAthleteInputModel atletaInputModel) {
        List<Subscription> subscriptions = new List<Subscription>();
        string newCpf = "";

        var atleta = await _athleteService.GetByIdAsync(id);

        if (atleta == null)
            return NotFound();

        try {
            await _athleteService.UpdateAsync(atleta, atletaInputModel);

            if (atleta.Cpf != atletaInputModel.Cpf) {
                newCpf = atletaInputModel.Cpf;

                await _athleteService.UpdateCpfAsync(atleta, newCpf);

                subscriptions = await _subscriptionService.GetAthleteSubscriptions(atleta.Cpf);

                if (subscriptions.Any())
                    await _subscriptionService.UpdateSubscriptionsCpfAthlete(subscriptions, newCpf);
            }

        } catch (Exception ex) {
            _logger.SaveLog("Erro ao atualizar atleta: ");
            _logger.SaveLog("Atleta: " + atleta.ToString());
            _logger.SaveLog("AtletaInputModel: " + JsonConvert.SerializeObject(atletaInputModel));
            _logger.SaveLog(ex.Message);

            return BadRequest(ex.Message);
        }

        return NoContent();
    }

    [HttpPut("atualizar-camisas/{cpf}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<IActionResult> UpdateShirtsAthlete([FromRoute] string cpf, UpdateShirtInputModel atletaInputModel) {
        var athlete = await _athleteService.GetByCpfAsync(cpf);

        if (athlete == null)
            return NotFound();

        await _athleteService.UpdateShirtAsync(athlete, atletaInputModel);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = "EmployeePolicy")]
    public async Task<IActionResult> DeleteAthlete([FromRoute] string id) {
        Athlete athlete;

        try {
            athlete = await _athleteService.GetByIdAsync(id);

            if (athlete == null)
                return NotFound(new { message = "Atleta não encontrado" });
        } catch {
            return BadRequest("Ocorreu um erro ao consultar o atleta");
        }

        try {
            await _athleteService.DeleteAsync(athlete);
        } catch {
            return BadRequest(new { message = "Ocorreu um erro ao deletar o atleta" });
        }

        return Ok();
    }
}

