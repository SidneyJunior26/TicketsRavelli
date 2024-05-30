using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TicketsRavelli.Application.InputModels.Atletas;
using TicketsRavelli.Application.Services.Interfaces;
using TicketsRavelli.Core.Entities.Athletes;
using TicketsRavelli.Endpoints.Security;
using TicketsRavelli.Infrastructure.EmailServices.Interfaces;
using TicketsRavelli.Infrastructure.Security.Interfaces;

namespace TicketsRavelli.Controllers.Secutiry;

[ApiController]
[Route("v1/seguranca")]
public class SecurityController : ControllerBase {
    private readonly ISegurancaService _segurancaService;
    private readonly IAthleteService _atletaService;
    private readonly ICryptographyService _criptografiaService;
    private readonly IUtilidadesService _utilidadesService;
    private readonly IEmailService _emailService;

    public SecurityController(ISegurancaService segurancaService,
        IAthleteService atletaService,
        ICryptographyService criptografiaService,
        IUtilidadesService utilidadesService, IEmailService emailService) {
        _segurancaService = segurancaService;
        _atletaService = atletaService;
        _criptografiaService = criptografiaService;
        _utilidadesService = utilidadesService;
        _emailService = emailService;
    }

    [HttpPost]
    [Route("login")]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginInputModel loginInputModel) {
        Athlete atleta;

        try {
            atleta = await _atletaService.GetByCpfAsync(loginInputModel.Cpf);
        } catch (Exception ex) {
            return BadRequest("Consulta = " + ex.Message);
        }


        if (atleta == null)
            return NotFound();

        try {

            if (atleta.Acesso != _criptografiaService.Criptografar(loginInputModel.Password))
                return Unauthorized();
        } catch (Exception ex) {
            return BadRequest("Criptografia = " + ex.Message);
        }

        try {
            var token = _segurancaService.Login(atleta, loginInputModel);

            return Ok(new {
                token = token
            });

        } catch (Exception ex) {
            return BadRequest("Login = " + ex.Message);
        }
    }

    [HttpPost("enviar-codigo/{cpfEmail}")]
    [AllowAnonymous]
    public async Task<IActionResult> EnviarCodigoPorEmail(string cpfEmail) {
        var atleta = await _atletaService.GetByCpfOrEmailAsync(cpfEmail);

        if (atleta == null)
            return NotFound(new { mensagem = "Nenhum cadastro encontrado" });

        var guid = Guid.NewGuid().ToString();

        var codigo = guid.Substring(0, guid.IndexOf("-"));

        try {
            await _segurancaService.SalvarCodigoSenha(codigo, atleta);

            _emailService.EnviarCodigoResetSenha(codigo, atleta);
        } catch (Exception ex) {
            return BadRequest(ex);
        }

        var emailEscondido = _utilidadesService.OcultarEmail(atleta.Email);

        return Ok(new { email = emailEscondido });
    }

    [HttpPost("enviar-codigo/id/{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> EnviarCodigoPorEmailId(string id) {
        var atleta = await _atletaService.GetByIdAsync(id);

        if (atleta == null)
            return NotFound();

        var guid = Guid.NewGuid().ToString();

        var codigo = guid.Substring(0, guid.IndexOf("-"));

        try {
            await _segurancaService.SalvarCodigoSenha(codigo, atleta);

            _emailService.EnviarCodigoResetSenha(codigo, atleta);
        } catch (Exception ex) {
            return BadRequest(ex);
        }

        return Ok(codigo);
    }

    [HttpGet("primeiro-acesso/{cpfAtleta}")]
    [AllowAnonymous]
    public async Task<IActionResult> VerificaPrimeiroAcesso(string cpfAtleta) {
        var atleta = await _atletaService.GetByCpfAsync(cpfAtleta);

        if (atleta == null)
            return NotFound();

        return Ok(new { id = atleta.Id, primeiroAcesso = atleta.PrimeiroAcesso });
    }

    [HttpPut("nova-senha")]
    [AllowAnonymous]
    public async Task<IActionResult> NovaSenha(NovaSenhaCodigoInputModel novaSenhaCodigoInputModel) {
        var atleta = await _atletaService.GetByIdAsync(novaSenhaCodigoInputModel.id);

        if (atleta == null)
            return NotFound();

        if (atleta.CodigoSenha != novaSenhaCodigoInputModel.codigo)
            return Unauthorized();

        await _segurancaService.DefinirPrimeiraSenha(atleta, novaSenhaCodigoInputModel);

        return Ok();
    }

    [HttpPut("manager/nova-senha")]
    [Authorize(Policy = "EmployeePolicy")]
    public async Task<IActionResult> NovaSenhaManager(ManagerNovaSenha managerNovaSenha) {
        var atleta = await _atletaService.GetByIdAsync(managerNovaSenha.id);

        if (atleta == null)
            return NotFound();

        try {
            await _segurancaService.AtualizarSenha(atleta, managerNovaSenha.novaSenha);

        } catch (Exception ex) {
            return BadRequest(ex.Message);
        }

        return Ok();
    }

    [HttpPut("alterar-senha")]
    [Authorize]
    public async Task<IActionResult> AtualizarSenha(NovaSenhaInputModel novaSenhaInputModel) {
        var atleta = await _atletaService.GetByCpfAsync(novaSenhaInputModel.cpf);

        if (atleta == null)
            return NotFound(new { mensagem = "Cadastro não encontrado" });

        if (atleta.Acesso != _criptografiaService.Criptografar(novaSenhaInputModel.senhaAtual))
            return BadRequest(new { mensagem = "Senha inválida" });

        await _segurancaService.AtualizarSenha(atleta, novaSenhaInputModel.novaSenha);

        return Ok();
    }

    [HttpPut("esqueci-senha")]
    [AllowAnonymous]
    public async Task<IActionResult> EsqueciSenha(EsqueceuSenhaInputModel esqueceuSenhaInputModel) {
        var atleta = await _atletaService.GetByCpfAsync(esqueceuSenhaInputModel.cpfEmail);

        if (atleta == null)
            return NotFound(new { mensagem = "CPF não cadastrado" });

        if (atleta.CodigoSenha != esqueceuSenhaInputModel.codigo)
            return BadRequest(new { mensagem = "Código inválido" });

        if (!_utilidadesService.SenhaValida(esqueceuSenhaInputModel.novaSenha))
            return BadRequest(new { mensagem = "Senha deve conter pelo menos 8 caracteres, um número, uma letra maiúscula, uma minúscula, e um caractere especial" });

        try {
            await _segurancaService.AtualizarSenha(atleta, esqueceuSenhaInputModel.novaSenha);
            return Ok();
        } catch {
            return BadRequest(new { erroInterno = "Ocorreu um erro ao atualizar a senha. Tente novamente" });
        }
    }

    [HttpGet("{codigo}/{cpf}")]
    [AllowAnonymous]
    public async Task<IActionResult> ValidarCodigoResetSenha(string codigo, string cpf) {
        var atleta = await _atletaService.GetByCpfAsync(cpf);

        if (atleta == null)
            return NotFound();

        if (!_segurancaService.ValidarCodigoResetSenha(codigo, atleta))
            return Unauthorized();

        return Ok();
    }
}

