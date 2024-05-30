using Microsoft.EntityFrameworkCore;
using TicketsRavelli.Core.Entities.Cortesias;
using TicketsRavelli.Infra.Data;
using TicketsRavelli.Infrastructure.Persistence.Repositories.Interfaces;

namespace TicketsRavelli.Infrastructure.Persistence.Repositories.Implementations;

public class CourtesyRepository : ICourtesyRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CourtesyRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateCouponEventAsync(Courtesy courtesy)
    {
        await _dbContext.Cortesias.AddAsync(courtesy);
    }

    public void DeleteCoupom(Courtesy courtesy)
    {
        _dbContext.Remove(courtesy);
    }

    public async Task<Courtesy?> QueryActiveCourtesyCouponEventAsync(int idEvent, string coupom)
    {
        return await _dbContext.Cortesias
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Cupom == coupom &&
                c.IdEvento == idEvent &&
                c.Ativo == 1);
    }

    public async Task<Courtesy> QueryCoupomCourtesyAsync(string coupom)
    {
        return await _dbContext.Cortesias.SingleOrDefaultAsync(c => c.Cupom == coupom);
    }

    public async Task<List<Courtesy>> QueryCouponsEventAsync(int idEvent)
    {
        return await _dbContext.Cortesias
                .AsNoTracking()
                .Where(c => c.IdEvento == idEvent)
                .ToListAsync();
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}

