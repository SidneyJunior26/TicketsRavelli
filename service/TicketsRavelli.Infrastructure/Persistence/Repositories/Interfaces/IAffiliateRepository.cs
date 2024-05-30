using TicketsRavelli.Core.Entities.Afiliados;

namespace TicketsRavelli.Infrastructure.Persistence.Repositories.Interfaces;

public interface IAffiliateRepository
{
    Task CreateAsync(Affiliate affiliate);
    Task SaveChangesAsync();
    Task<List<Affiliate>> QueryAllAsync();
    Task<Affiliate> QueryByIdAsync(string id);
    Task<Affiliate> QueryByCpfAsync(string cpf);
    Task<List<Affiliate>> QueryByIdEventAsync(int idEvento);
    void Delete(Affiliate afiliado);
}