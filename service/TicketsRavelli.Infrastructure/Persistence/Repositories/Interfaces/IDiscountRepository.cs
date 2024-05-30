using TicketsRavelli.Core.Entities.Descontos;

namespace TicketsRavelli.Infrastructure.Persistence.Repositories.Interfaces;

public interface IDiscountRepository
{
    Task SaveChangesAsync();
    Task CreateAsync(Desconto discount);
    Task<Desconto> QueryByIdAsync(int id);
    Task<List<Desconto>> QueryByEventAsync(int idEvent);
    void Delete(Desconto discount);
    Task<Desconto> QueryValidateAsync(string coupom, int idEvent);
}