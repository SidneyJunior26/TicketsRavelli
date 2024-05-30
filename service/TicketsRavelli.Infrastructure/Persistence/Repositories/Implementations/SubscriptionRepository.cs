using Microsoft.EntityFrameworkCore;
using TicketsRavelli.Core.Entities.Eventos;
using TicketsRavelli.Infra.Data;
using TicketsRavelli.Infrastructure.Persistence.Repositories.Interfaces;

namespace TicketsRavelli.Infrastructure.Persistence.Repositories.Implementations
{
    public class SubscriptionRepository : ISubscriptionRepository {
        private readonly ApplicationDbContext _context;

        public SubscriptionRepository(ApplicationDbContext context) {
            _context = context;
        }

        public void UpdateSubscriptionState(Subscription inscricaoEvento) {
            _context.Entry(inscricaoEvento).State = EntityState.Modified;
        }

        public async Task CreateAsync(Subscription inscricaoEvento) {
            await _context.Inscricoes.AddAsync(inscricaoEvento);
        }

        public async Task<Subscription?> QueryChargeIdAsync(int chargeId) {
            return await _context.Inscricoes
                .SingleOrDefaultAsync(i => i.GnChargeId == chargeId);
        }

        public async Task<Subscription> QuerySubscriptionByIdAsync(int id) {
            return await _context.Inscricoes
                .Include(i => i.Evento)
                .Include(i => i.Atleta)
                .SingleOrDefaultAsync(i => i.Id == id);
        }

        public async Task<List<Subscription>> QueryAllSubscriptionsAsync() {
            return await _context.Inscricoes
                .AsNoTracking()
                .Include(i => i.Evento)
                .Include(i => i.Atleta)
                .Include(i => i.Subcategoria)
                .ToListAsync();
        }

        public async Task<List<Subscription>> QueryAthleteSubscriptionsAsync(string cpfAtleta) {
            return await _context.Inscricoes.Where(i => i.CpfAtleta == cpfAtleta)
                .AsNoTracking()
                .Include(i => i.Evento)
                .Include(i => i.Subcategoria)
                .OrderByDescending(i => i.Id)
                .ToListAsync();
        }

        public async Task<List<Subscription>> QueryUnpaidBilletSubscriptionsAsync(int idEvento) {
            return await _context.Inscricoes.Where(i => i.Pago == false &&
                i.GnChargeId != null &&
                i.GnStatus != "unpaid" &&
                i.IdEvento == idEvento)
                .ToListAsync();
        }

        public async Task<List<Subscription>> QueryRegisteredSubscriptionsForEventAsync(int idEvento) {
            return await _context.Inscricoes
                .AsNoTracking()
                .Include(i => i.Evento)
                .Include(i => i.Atleta)
                .Include(i => i.Subcategoria)
                .Where(i => i.IdEvento == idEvento && i.DataEfetivacao != null)
                .ToListAsync();
        }

        public async Task<List<Subscription>> QuerySubscriptionsForEventAsync(int idEvento) {
            return await _context.Inscricoes
                .AsNoTracking()
                .Include(i => i.Evento)
                .Include(i => i.Atleta)
                .Include(i => i.Subcategoria)
                .Where(i => i.IdEvento == idEvento)
                .OrderBy(i => i.Subcategoria.DescSubcategoria)
                .ToListAsync();
        }

        public async Task<List<Subscription>> QueryNonEffectiveSubscriptionsForEventAsync(int idEvento) {
            return await _context.Inscricoes
                .AsNoTracking()
                .Include(i => i.Evento)
                .Include(i => i.Atleta)
                .Include(i => i.Subcategoria)
                .Where(i => i.IdEvento == idEvento && i.Pago == false)
                .ToListAsync();
        }

        public async Task<List<Subscription>> QueryUnpaidPixSubscriptionsAsync(int idEvento) {
            return await _context.Inscricoes.Where(i => i.Pago == false &&
                i.GnChargeTxId != null &&
                i.GnStatus != "CANCELADA" &&
                i.GnStatus != "CONCLUIDA" &&
                i.IdEvento == idEvento)
                .ToListAsync();
        }

        public void DeleteAsync(Subscription inscricaoEvento) {
            _context.Inscricoes.Remove(inscricaoEvento);
        }

        public async Task SaveChangesAsync() {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> QueryCheckIfAthleteRegisteredForEventAsync(string cpfAtleta, int idEvento) {
            return await _context.Inscricoes.Where(i => i.CpfAtleta == cpfAtleta &&
                i.IdEvento == idEvento).AnyAsync();
        }

        public async Task<List<Subscription>> QueryAllSubscriptionsWithEventAsync()
        {
            return await _context.Inscricoes
                .AsNoTracking()
                .OrderByDescending(i => i.IdEvento)
                .Include(i => i.Evento)
                .Include(i => i.Atleta)
                .Include(i => i.Subcategoria)
                .ToListAsync();
        }
    }
}

