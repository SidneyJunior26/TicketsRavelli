using TicketsRavelli.Core.Entities.Eventos;

namespace TicketsRavelli.Infrastructure.Persistence.Repositories.Interfaces;

public interface IEventRepository
{
    Task<List<Evento>> QueryAllEventsAsync();
    Task<List<Evento>> QueryAllActivesAsync();
    Task<List<Evento>> QueryUpComingAsync();
    Task<Evento?> QueryByIdAsync(int id);
    void Delete(Evento evento);
    Task CreateAsync(Evento newEvent);
    Task UpdateImageAsync(Evento evento, string nome);
    Task SaveChangesAsync();
}