using TicketsRavelli.Controllers.Regulamentos;
using TicketsRavelli.Core.Entities.Eventos;

namespace TicketsRavelli.Application.Services.Interfaces
{
    public interface IRegulationService {
        Task<Regulamento> GetByEventAsync(int idEvento);
        Task CreateAsync(NewRegulationInputModel regulamentoInputModel);
        Task UpdateAsync(Regulamento regulamento, UpdateRegulationInputModel regulamentoInputModel);
    }
}

