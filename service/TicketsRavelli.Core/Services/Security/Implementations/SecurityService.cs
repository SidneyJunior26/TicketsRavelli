using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using TicketsRavelli.Infrastructure.Security.Interfaces;

namespace TicketsRavelli.Services.Security;

public class SecurityService : ISecurityService {
    private readonly byte[] _key;
    private readonly string _audience;
    private readonly string _issuer;

    public SecurityService(IConfiguration config) {
        _key = Encoding.ASCII.GetBytes(config["JwtBearerTokenSettings:SecretKey"]!);
        _audience = config["JwtBearerTokenSettings:Audience"]!;
        _issuer = config["JwtBearerTokenSettings:Issuer"]!;
    }

    public SecurityTokenDescriptor GetTokenDescriptor(ClaimsIdentity subject) {
        return new SecurityTokenDescriptor {
            Subject = subject,
            SigningCredentials =
            new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature),
            Audience = _audience,
            Issuer = _issuer,
            Expires = DateTime.UtcNow.AddHours(1),
        };
    }
}

