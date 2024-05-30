using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using TicketsRavelli.Application.Services.Interfaces;
using TicketsRavelli.Core.Entities.Athletes;
using TicketsRavelli.Endpoints.Security;
using TicketsRavelli.Infra.Data;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using TicketsRavelli.Infrastructure.Security.Interfaces;
using TicketsRavelli.Infrastructure.EmailServices.Interfaces;
using TicketsRavelli.Application.InputModels.Atletas;

namespace TicketsRavelli.Application.Services.Implementations
{
    public class SegurancaService : ISegurancaService {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly ICryptographyService _criptografiaService;

        public SegurancaService(ApplicationDbContext context,
            IConfiguration configuration,
            IEmailService emailService,
            ICryptographyService criptografiaService) {
            _context = context;
            _config = configuration;
            _criptografiaService = criptografiaService;
        }

        public async Task AtualizarSenha(Athlete atleta, string novaSenha) {
            var novaSenhaCrip = _criptografiaService.Criptografar(novaSenha);

            atleta.AtualizarSenha(novaSenhaCrip);

            await _context.SaveChangesAsync();
        }

        public async Task DefinirPrimeiraSenha(Athlete atleta, NovaSenhaCodigoInputModel novaSenhaCodigoInputModel) {
            atleta.DefinirPrimeiraSenha(_criptografiaService.Criptografar(novaSenhaCodigoInputModel.novaSenha));

            await _context.SaveChangesAsync();
        }

        public async Task SalvarCodigoSenha(string codigo, Athlete atleta) {
            atleta.SalvarCodigoSenha(codigo);

            await _context.SaveChangesAsync();
        }

        public string Login(Athlete atleta, LoginInputModel loginInputModel) {
            var subject = new ClaimsIdentity(new Claim[]
            {
                new Claim("ID", atleta.Id),
                new Claim("cpf", loginInputModel.Cpf),
                new Claim("Name", atleta.Nome)
            });

            if (atleta.Nivel == 2)
                subject.AddClaim(new Claim("employee", loginInputModel.Cpf));

            if (atleta.Nivel == 3)
                subject.AddClaim(new Claim("integracao", "aws"));

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["JwtBearerTokenSettings:SecretKey"]);
            var tokenDescription = new SecurityTokenDescriptor {
                Subject = subject,
                Audience = _config["JwtBearerTokenSettings:Audience"],
                Issuer = _config["JwtBearerTokenSettings:Issuer"],
                Expires = DateTime.UtcNow.AddDays(4),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescription);

            return tokenHandler.WriteToken(token);
        }

        public bool ValidarCodigoResetSenha(string codigo, Athlete atleta) {
            return atleta.CodigoSenha == codigo;
        }
    }
}

