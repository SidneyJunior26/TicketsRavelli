using Microsoft.EntityFrameworkCore;
using TicketsRavelli.Core.Entities.Eventos;
using TicketsRavelli.Infra.Data;
using TicketsRavelli.Infrastructure.Persistence.Repositories.Interfaces;

namespace TicketsRavelli.Infrastructure.Persistence.Repositories.Implementations;

public class EventRepsitory : IEventRepository {
    private readonly ApplicationDbContext _context;
    public EventRepsitory(ApplicationDbContext context) {
        _context = context;
    }

    public async Task CreateAsync(Evento newEvent) {
        await _context.Eventos.AddAsync(newEvent);
    }

    public void Delete(Evento evento) {
        _context.Eventos.Remove(evento);
    }

    public async Task<List<Evento>> QueryAllActivesAsync() {
        return await _context.Eventos
                .AsNoTracking()
                .Where(e => e.AtivaEvento == 1 &&
                            e.AtivaInscricao == 1 &&
                            e.DataFimInscricao >= DateTime.Today &&
                            e.DataIniInscricao <= DateTime.Today)
                .ToListAsync();
    }

    public async Task<Evento?> QueryByIdAsync(int id) {
        return await _context.Eventos
                .SingleOrDefaultAsync(e => e.Id == id);
    }

    public async Task<List<Evento>> QueryAllEventsAsync() {
        return await _context.Eventos
                .AsNoTracking()
                .OrderByDescending(e => e.Data)
                .ToListAsync();
    }

    public async Task<List<Evento>> QueryUpComingAsync() {
        return await _context.Eventos
                .AsNoTracking()
                .Where(e => e.DataIniInscricao > DateTime.Now && e.AtivaEvento == 1)
                .OrderByDescending(e => e.DataIniInscricao)
                .ToListAsync();
    }

    public async Task SaveChangesAsync() {
        await _context.SaveChangesAsync();
    }

    public Task UpdateImageAsync(Evento evento, string nome) {
        throw new NotImplementedException();
    }
}

