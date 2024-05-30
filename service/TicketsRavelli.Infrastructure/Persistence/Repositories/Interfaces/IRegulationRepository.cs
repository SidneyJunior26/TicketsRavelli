using TicketsRavelli.Core.Entities.Eventos;

namespace TicketsRavelli.Infrastructure.Persistence.Repositories.Interfaces;

public interface IRegulationRepository
{
    Task SaveChangesAsync();
    Task CreateAsync(Regulamento regulation);
    Task<Regulamento> QueryByEventAsync(int idEvent);
}