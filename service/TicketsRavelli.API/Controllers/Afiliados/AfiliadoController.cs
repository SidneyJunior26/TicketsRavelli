using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketsRavelli.Application.InputModels;
using TicketsRavelli.Application.Services.Interfaces;

namespace TicketsRavelli.API.Controllers.Afiliados;

[ApiController]
[Route("v1/[controller]")]
public class AfiliadoController : ControllerBase
{
    private readonly IAffiliateService _affiliateService;

    public AfiliadoController(IAffiliateService affiliateService)
    {
        _affiliateService = affiliateService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = "EmployeePolicy")]
    public async Task<IActionResult> GetAffiliates()
    {
        var afiliates = await _affiliateService.GetAllAsync();

        if (!afiliates.Any())
            return NotFound();

        return Ok(afiliates);
    }

    [HttpGet("{cpf}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = "EmployeePolicy")]
    public async Task<IActionResult> GetAffiliateByCpf(string cpf)
    {
        var affiliate = await _affiliateService.GetByCpfAsync(cpf);

        if (affiliate == null)
            return NotFound();

        return Ok(affiliate);
    }

    [HttpGet("evento/{idEvento}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = "EmployeePolicy")]
    public async Task<IActionResult> GetAffiliateByIdEvent(int idEvent)
    {
        var affiliatesEvent = await _affiliateService.GetByIdEventAsync(idEvent);

        if (affiliatesEvent == null)
            return NotFound();

        return Ok(affiliatesEvent);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [Authorize(Policy = "EmployeePolicy")]
    public async Task<IActionResult> PostAffiliate(AffiliateInputModel affiliateInputModel)
    {
        var affiliate = await _affiliateService.GetByCpfAsync(affiliateInputModel.cpf);

        if (affiliate != null)
            return BadRequest(new { mensagem = "Afiliado já cadastrado" });

        await _affiliateService.CreateAsync(affiliateInputModel);

        return Ok();
    }

    [HttpPut("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = "EmployeePolicy")]
    public async Task<IActionResult> UpdateAffiliate(string id, AffiliateInputModel afiliadoInput)
    {
        var affiliate = await _affiliateService.GetByIdAsync(id);

        if (affiliate == null)
            return NotFound();

        await _affiliateService.UpdateAsync(affiliate, afiliadoInput);

        return NoContent();
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [Authorize(Policy = "EmployeePolicy")]
    public async Task<IActionResult> DeleteAffiliate(string id)
    {
        var affiliate = await _affiliateService.GetByIdAsync(id);

        if (affiliate == null)
            return NotFound();

        await _affiliateService.Delete(affiliate);

        return Ok();
    }
}