using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketsRavelli.Application.InputModels.Eventos;
using TicketsRavelli.Application.Services.Interfaces;

namespace TicketsRavelli.API.Controllers.Eventos
{
    [ApiController]
    [Route("v1/[controller]")]
    public class CortesiasController : ControllerBase
    {
        private readonly ICourtesyService _courtesyService;
        private readonly IEventService _eventService;

        public CortesiasController(ICourtesyService courtesyService, IEventService eventService)
        {
            _courtesyService = courtesyService;
            _eventService = eventService;
        }

        [HttpGet("validar/{idEvento}/{cupom}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [AllowAnonymous]
        public async Task<IActionResult> ValidateCoupom(int idEvento, string cupom)
        {
            var courtesy = await _courtesyService.GetActiveCourtesyCouponEventAsync(idEvento, cupom);

            if (courtesy == null)
                return NotFound(new { message = "Este cupom não é válido" });

            return Ok();
        }

        [HttpGet("evento/{idEvento}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = "EmployeePolicy")]
        public async Task<IActionResult> GetCouponsEvent(int idEvento)
        {
            var coupons = await _courtesyService.GetCouponsEventAsync(idEvento);

            if (coupons == null)
                return NotFound(new { message = "Este cupom não é válido" });

            return Ok(coupons);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = "EmployeePolicy")]
        public async Task<IActionResult> GerarCuponsCortesia([FromBody] NovasCortesiasInputModel novasCortesias)
        {
            var eventById = await _eventService.GetByIdAsync(novasCortesias.idEvento);

            if (eventById == null)
                return NotFound();

            var coupom = await _courtesyService.CreateCouponEventAsync(eventById.Id);

            return Ok(new { cupom = coupom });
        }

        [HttpPut("desativar/{idEvento}/{cupom}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = "EmployeePolicy")]
        public async Task<IActionResult> DisableCoupom(int idEvento, string cupom)
        {
            var courtesy = await _courtesyService.GetActiveCourtesyCouponEventAsync(idEvento, cupom);

            if (courtesy == null)
                return NotFound(new { messase = "Cupom não encontrado" });

            await _courtesyService.DisableCoupomAsync(courtesy);

            return NoContent();
        }

        [HttpPut("status/{cupom}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = "EmployeePolicy")]
        public async Task<IActionResult> UpdateStatus(string cupom)
        {
            var courtesy = await _courtesyService.GetCoupomCourtesyAsync(cupom);

            if (courtesy == null)
                return NotFound(new { messase = "Cupom não encontrado" });

            await _courtesyService.UpdateStatusCoupomAsync(courtesy);

            return NoContent();
        }

        [HttpDelete("{cupom}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [Authorize(Policy = "EmployeePolicy")]
        public async Task<IActionResult> DeleteCoupom(string cupom)
        {
            var courtesy = await _courtesyService.GetCoupomCourtesyAsync(cupom);

            if (courtesy == null)
                return NotFound(new { messase = "Cupom não encontrado" });

            await _courtesyService.DeleteCoupomAsync(courtesy);

            return NoContent();
        }
    }
}