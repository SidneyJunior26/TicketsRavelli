using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketsRavelli.Application.Services.Interfaces;

namespace TicketsRavelli.Controllers.RegistrosMedicos;

[ApiController]
[Route("v1/[controller]")]
public class RegistrosMedicosController : ControllerBase
{
    private readonly IMedicalDataService _medicalDataService;
    private readonly ILogSystem _logger;

    public RegistrosMedicosController(IMedicalDataService medicalDataService, ILogSystem logger)
    {
        _medicalDataService = medicalDataService;
        _logger = logger;
    }

    [HttpGet("{idAtleta}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize]
    public async Task<IActionResult> GetByIdAthlete(string idAtleta)
    {
        var response = await _medicalDataService.GetByAthleteAsync(idAtleta);

        if (response == null)
            return NotFound();

        return Ok(response);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [AllowAnonymous]
    public async Task<IActionResult> Post(MedicalDataInputModel medicalDataInputModel)
    {
        try
        {
            await _medicalDataService.CreateAsync(medicalDataInputModel);

            return Ok();
        }
        catch (Exception ex)
        {
            _logger.SaveLog(ex.Message);
            _logger.SaveLog(ex.InnerException.ToString());
            _logger.SaveLog("Registro: " + medicalDataInputModel);

            return BadRequest(new { mensagem = "Ocorreu um erro ao cadastrar os dados médicos" });
        }
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize]
    public async Task<IActionResult> AtualizarRegistroMedico(MedicalDataInputModel registroMedicoInputModel)
    {
        try
        {
            var medicalData = await _medicalDataService.GetByAthleteAsync(registroMedicoInputModel.IdAtleta);

            if (medicalData == null)
                return NotFound();

            await _medicalDataService.UpdateAsync(medicalData, registroMedicoInputModel);

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.SaveLog(ex.Message);
            _logger.SaveLog(ex.InnerException.ToString());
            _logger.SaveLog("Registro: " + registroMedicoInputModel);

            return BadRequest(new { mensagem = "Ocorreu um erro ao cadastrar os dados médicos" });
        }
    }
}