using TicketsRavelli.Application.ViewModels.Eventos;
using TicketsRavelli.Controllers.Eventos;
using TicketsRavelli.Core.Entities.Eventos;

namespace TicketsRavelli.Application.Services.Interfaces
{
    public interface IEventService {
        Task<List<EventoComEstatisticasInscricao>> GetEventsWithStatisticsAsync();
        Task<List<Evento>> GetAllEventsActivesAsync();
        Task<List<Evento>> GetUpComingEventsAsync();
        Task<Evento?> GetEventByIdAsync(int id);
        Task<PacotesViewModel> GetPackagesAsync(int eventoId);
        Task DeleteEventAsync(Evento evento);
        Task<int> CreateEventAsync(EventInputModel eventoInputModel);
        Task UpdateEventAsync(Evento evento, EventInputModel eventoInputModel);
        Task<decimal> GetPackageValueAsync(int? pacote, int eventoId);
        Task UpdateImageAsync(Evento evento, string nome);
    }
}

