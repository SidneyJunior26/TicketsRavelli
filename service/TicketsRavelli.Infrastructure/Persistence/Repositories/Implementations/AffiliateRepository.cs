using Microsoft.EntityFrameworkCore;
using TicketsRavelli.Core.Entities.Afiliados;
using TicketsRavelli.Infra.Data;
using TicketsRavelli.Infrastructure.Persistence.Repositories.Interfaces;

namespace TicketsRavelli.Infrastructure.Persistence.Repositories.Implementations;

public class AffiliateRepository : IAffiliateRepository
{
    private readonly ApplicationDbContext _context;

    public AffiliateRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Affiliate affiliate)
    {
        await _context.Afiliados.AddAsync(affiliate);
    }

    public void Delete(Affiliate affiliate)
    {
        _context.Remove(affiliate);
    }

    public async Task<Affiliate> QueryByCpfAsync(string cpf)
    {
        return await _context.Afiliados.SingleOrDefaultAsync(a => a.Cpf == cpf);
    }

    public async Task<Affiliate> QueryByIdAsync(string id)
    {
        return await _context.Afiliados.SingleOrDefaultAsync(a => a.Id == id);
    }

    public async Task<List<Affiliate>> QueryByIdEventAsync(int idEvento)
    {
        return await _context.Afiliados
            .AsNoTracking()
            .Where(a => a.Inscricoes.Any(i => i.IdEvento == idEvento))
            .Include(a => a.Inscricoes)
            .ThenInclude(i => i.Atleta)
            .ToListAsync();
    }

    public async Task<List<Affiliate>> QueryAllAsync()
    {
        return await _context.Afiliados.ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}