using Microsoft.EntityFrameworkCore;
using TicketsRavelli.Core.Entities.Eventos;
using TicketsRavelli.Infra.Data;
using TicketsRavelli.Infrastructure.Persistence.Repositories.Interfaces;

namespace TicketsRavelli.Infrastructure.Persistence.Repositories.Implementations;

public class ReportRepository : IReportRepository
{
    private readonly ApplicationDbContext _context;

    public ReportRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<(string CamisaCiclismo, int Quantidade)>> GetCyclingShirtsAsync(int idEvent)
    {
        var result = await _context.Inscricoes
            .AsNoTracking()
            .Include(i => i.Evento)
            .Include(i => i.Atleta)
            .Include(i => i.Subcategoria)
            .Where(i => i.IdEvento == idEvent && i.Atleta.CamisaCiclismo != null && (i.Pacote == 2 || i.Pacote == 3))
            .GroupBy(i => i.Atleta.CamisaCiclismo)
            .Select(group => new
            {
                CamisaCiclismo = "Camisa Ciclismo: " + group.Key,
                Quantidade = group.Count()
            })
            .OrderBy(result => result.Quantidade)
            .ToListAsync();

        return result.Select(r => (r.CamisaCiclismo, r.Quantidade)).ToList();
    }

    public async Task<List<(string Camisa, int Quantidade)>> GetNormalShirtsAsync(int idEvent)
    {
        var result = await _context.Inscricoes
                .AsNoTracking()
                .Include(i => i.Evento)
                .Include(i => i.Atleta)
                .Include(i => i.Subcategoria)
                .Where(i => i.IdEvento == idEvent && i.Atleta.Camisa != null && i.Pacote == 1)
                .GroupBy(i => i.Atleta.Camisa)
                .Select(group => new
                {
                    Camisa = "Camisa Normal: " + group.Key,
                    Quantidade = group.Count()
                })
                .OrderBy(result => result.Quantidade)
                .ToListAsync();

        return result.Select(r => (r.Camisa, r.Quantidade)).ToList();
    }

    public async Task<List<Subscription>> GetSubscriptions(int idEvent)
    {
        return await _context.Inscricoes
                .AsNoTracking()
                .Include(i => i.Evento)
                .Include(i => i.Atleta).ThenInclude(a => a.RegistroMedico)
                .Include(i => i.Subcategoria)
                .Where(i => i.IdEvento == idEvent)
                .OrderBy(i => i.DataEfetivacao).ToListAsync();
    }

    public async Task<List<(string DescricaoSubcategoria, int Quantidade)>> GetSubscriptionsGroupByCategories(int idEvent)
    {
        var result = await _context.Inscricoes
                .AsNoTracking()
                .Include(i => i.Evento)
                .Include(i => i.Atleta)
                .Include(i => i.Subcategoria)
                .Where(i => i.IdEvento == idEvent)
                .GroupBy(i => i.Subcategoria.DescSubcategoria)
                .Select(group => new
                {
                    DescricaoSubcategoria = group.Key,
                    Quantidade = group.Count()
                })
                .OrderBy(result => result.DescricaoSubcategoria)
                .ToListAsync();

        return result.Select(r => (r.DescricaoSubcategoria, r.Quantidade)).ToList();
    }
}

