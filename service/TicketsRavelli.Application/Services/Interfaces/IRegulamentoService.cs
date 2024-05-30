using TicketsRavelli.Controllers.Regulamentos;
using TicketsRavelli.Core.Entities.Eventos;

namespace TicketsRavelli.Application.Services.Interfaces
{
    public interface IRegulamentoService {
        Task<Regulamento> ConsultarRegulamentoIdEvento(int idEvento);
        Task CadastrarRegulamento(NovoRegulamentoInputModel regulamentoInputModel);
        Task AtualizarRegulamento(Regulamento regulamento, EditarRegulamentoInputModel regulamentoInputModel);
    }
}

