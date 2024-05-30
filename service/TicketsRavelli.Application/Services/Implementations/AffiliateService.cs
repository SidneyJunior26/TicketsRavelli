using TicketsRavelli.Application.InputModels;
using TicketsRavelli.Application.Services.Interfaces;
using TicketsRavelli.Core.Entities.Afiliados;
using TicketsRavelli.Infrastructure.Persistence.Repositories.Interfaces;

namespace TicketsRavelli.Application.Services.Implementations;

public class AffiliateService : IAffiliateService {
    private readonly IAffiliateRepository _affiliateRepository;

    public AffiliateService(IAffiliateRepository affiliateRepository)
    {
        _affiliateRepository = affiliateRepository;
    }

    public async Task UpdateAsync(Affiliate affiliate, AffiliateInputModel affiliateInputModel) {
        affiliate.Update(affiliateInputModel.cpf, affiliateInputModel.nome, affiliateInputModel.porcentagem);

        await _affiliateRepository.SaveChangesAsync();
    }

    public async Task CreateAsync(AffiliateInputModel afiliado) {
        var newAffiliate = new Affiliate(afiliado.cpf, afiliado.nome, afiliado.porcentagem);

        await _affiliateRepository.CreateAsync(newAffiliate);
        await _affiliateRepository.SaveChangesAsync();
    }

    public async Task<Affiliate> GetByCpfAsync(string cpf)
    {
        return await _affiliateRepository.QueryByCpfAsync(cpf);
    }

    public async Task<Affiliate> GetByIdAsync(string id)
    {
        return await _affiliateRepository.QueryByIdAsync(id);
    }

    public async Task<List<Affiliate>> GetAllAsync() {
        return await _affiliateRepository.QueryAllAsync();
    }

    public async Task<List<Affiliate>> GetByIdEventAsync(int idEvento) {
        return await _affiliateRepository.QueryByIdEventAsync(idEvento);
    }

    public async Task Delete(Affiliate afiliado)
    {
        _affiliateRepository.Delete(afiliado);

        await _affiliateRepository.SaveChangesAsync();
    }
}

