using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketsRavelli.Application.Services.Interfaces;
using TicketsRavelli.Infrastructure.EmailServices.Interfaces;

namespace TicketsRavelli.API.Controllers.Testes;

[ApiController]
[Route("v1/[controller]")]
public class TestesControlller : ControllerBase {
    private readonly IEmailService _emailService;
    private readonly ILogSystem _logger;

    public TestesControlller(IEmailService emailService, ILogSystem logger) {
        _emailService = emailService;
        _logger = logger;
    }

    [HttpPost]
    [Authorize(Policy = "EmployeePolicy")]
    public async Task<IActionResult> TesteEnvioEmail() {
        try {
            _emailService.TesteEmail();

        } catch (Exception ex) {
            _logger.SaveLog(ex.Message);
            _logger.SaveLog(ex.InnerException.ToString());
            return BadRequest(ex.Message);
        }

        return Ok();
    }
}