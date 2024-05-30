using TicketsRavelli.Core.Entities.Eventos;

namespace TicketsRavelli.Infrastructure.Persistence.Repositories.Interfaces;

public interface IReportRepository
{
    Task<List<Subscription>> GetSubscriptions(int idEvent);
    Task<List<(string DescricaoSubcategoria, int Quantidade)>> GetSubscriptionsGroupByCategories(int idEvent);
    Task<List<(string Camisa, int Quantidade)>> GetNormalShirtsAsync(int idEvent);
    Task<List<(string CamisaCiclismo, int Quantidade)>> GetCyclingShirtsAsync(int idEvent);
}