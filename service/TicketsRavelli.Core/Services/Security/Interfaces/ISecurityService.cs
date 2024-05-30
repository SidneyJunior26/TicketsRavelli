using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;

namespace TicketsRavelli.Infrastructure.Security.Interfaces
{
    public interface ISecurityService {
        SecurityTokenDescriptor GetTokenDescriptor(ClaimsIdentity subject);
    }
}

