using Microsoft.EntityFrameworkCore;
using TicketsRavelli.Core.Entities.Descontos;
using TicketsRavelli.Infra.Data;
using TicketsRavelli.Infrastructure.Persistence.Repositories.Interfaces;

namespace TicketsRavelli.Infrastructure.Persistence.Repositories.Implementations;

public class DiscountRepository : IDiscountRepository
{
    private readonly ApplicationDbContext _dbContext;

    public DiscountRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task CreateAsync(Desconto discount)
    {
        await _dbContext.Descontos.AddAsync(discount);
    }

    public void Delete(Desconto discount)
    {
        _dbContext.Remove(discount);
    }

    public async Task<List<Desconto>> QueryByEventAsync(int idEvent)
    {
        return await _dbContext.Descontos.AsNoTracking().Where(d => d.IdEvento == idEvent).ToListAsync();
    }

    public async Task<Desconto> QueryByIdAsync(int id)
    {
        return await _dbContext.Descontos.SingleOrDefaultAsync(d => d.Id == id);
    }

    public async Task<Desconto> QueryValidateAsync(string coupom, int idEvent)
    {
        return await _dbContext.Descontos
                .AsNoTracking()
                .SingleOrDefaultAsync(d => d.Cupom == coupom &&
                d.IdEvento == idEvent &&
                d.Ativo == 1);
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
}

