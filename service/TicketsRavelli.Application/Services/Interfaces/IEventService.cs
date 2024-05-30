using TicketsRavelli.Application.ViewModels.Eventos;
using TicketsRavelli.Controllers.Eventos;
using TicketsRavelli.Core.Entities.Eventos;

namespace TicketsRavelli.Application.Services.Interfaces
{
    public interface IEventService {
        Task<List<EventoComEstatisticasInscricao>> GetAllWithStatisticsAsync();
        Task<List<Evento>> GetAllActivesAsync();
        Task<List<Evento>> GetUpComingAsync();
        Task<Evento?> GetByIdAsync(int id);
        Task<PacotesViewModel> GetPackagesAsync(int eventoId);
        Task DeleteAsync(Evento evento);
        Task<int> CreateAsync(EventInputModel eventoInputModel);
        Task UpdateAsync(Evento evento, EventInputModel eventoInputModel);
        Task<decimal> GetPackageValueAsync(int? pacote, int eventoId);
        Task UpdateImageAsync(Evento evento, string nome);
    }
}