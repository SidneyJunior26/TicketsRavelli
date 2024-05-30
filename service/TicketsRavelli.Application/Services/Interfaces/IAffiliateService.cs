using TicketsRavelli.Application.InputModels;
using TicketsRavelli.Core.Entities.Afiliados;

namespace TicketsRavelli.Application.Services.Interfaces;

public interface IAffiliateService {
    Task CreateAsync(AffiliateInputModel afiliado);
    Task UpdateAsync(Affiliate afiliado, AffiliateInputModel afiliadoInput);
    Task<List<Affiliate>> GetAllAsync();
    Task<Affiliate> GetByIdAsync(string id);
    Task<Affiliate> GetByCpfAsync(string cpf);
    Task<List<Affiliate>> GetByIdEventAsync(int idEvento);
    Task Delete(Affiliate afiliado);
}

