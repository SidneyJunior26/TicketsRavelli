using Microsoft.EntityFrameworkCore;
using TicketsRavelli.Core.Entities.Eventos;
using TicketsRavelli.Infra.Data;
using TicketsRavelli.Infrastructure.Persistence.Repositories.Interfaces;

namespace TicketsRavelli.Infrastructure.Persistence.Repositories.Implementations;

public class RegulationRepository : IRegulationRepository
{
    private readonly ApplicationDbContext _context;

    public RegulationRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task CreateAsync(Regulamento regulation)
    {
        await _context.Regulamentos.AddAsync(regulation);
    }

    public async Task<Regulamento> QueryByEventAsync(int idEvent)
    {
        return await _context.Regulamentos
                .SingleOrDefaultAsync(e => e.IdEvento == idEvent);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}

