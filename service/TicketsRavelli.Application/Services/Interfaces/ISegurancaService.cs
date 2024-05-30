using TicketsRavelli.Application.InputModels.Atletas;
using TicketsRavelli.Core.Entities.Athletes;
using TicketsRavelli.Endpoints.Security;

namespace TicketsRavelli.Application.Services.Interfaces
{
    public interface ISegurancaService {
        string Login(Athlete atleta, LoginInputModel loginInputModel);
        Task DefinirPrimeiraSenha(Athlete atleta, NovaSenhaCodigoInputModel novaSenhaCodigoInputModel);
        Task AtualizarSenha(Athlete atleta, string novaSenha);
        bool ValidarCodigoResetSenha(string codigo, Athlete atleta);
        Task SalvarCodigoSenha(string codigo, Athlete atleta);
    }
}

