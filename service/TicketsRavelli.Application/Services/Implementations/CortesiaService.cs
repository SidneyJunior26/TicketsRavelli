using Microsoft.EntityFrameworkCore;
using TicketsRavelli.Application.Services.Interfaces;
using TicketsRavelli.Core.Entities.Cortesias;
using TicketsRavelli.Infra.Data;

namespace TicketsRavelli.Application.Services.Implementations {
    public class CortesiaService : ICortesiaService {
        private readonly ApplicationDbContext _dbContext;

        public CortesiaService(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        public async Task AlterarStatusCupom(Cortesia cortesia)
        {
            cortesia.AlterarStatus();

            await _dbContext.SaveChangesAsync();
        }

        public async Task CadastrarCortesiaEventoAsync(string cupom, int idEvento) {
            var cortesia = new Cortesia(idEvento, cupom.ToString());

            await _dbContext.Cortesias.AddAsync(cortesia);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Cortesia> ConsultarCupomCortesia(string cupom)
        {
            return await _dbContext.Cortesias.SingleOrDefaultAsync(c => c.Cupom == cupom);
        }

        public async Task<Cortesia?> ConsultarCupomCortesiaAtivoEventoAsync(int idEvento, string cupom)
        {
            return await _dbContext.Cortesias
                .AsNoTracking()
                .SingleOrDefaultAsync(c => c.Cupom == cupom &&
                c.IdEvento == idEvento &&
                c.Ativo == 1);
        }

        public async Task<List<Cortesia>> ConsultarCuponsEvento(int IdEvento)
        {
            return await _dbContext.Cortesias
                .AsNoTracking()
                .Where(c => c.IdEvento == IdEvento)
                .ToListAsync();
        }

        public async Task DesativarCupom(Cortesia cortesia) {
            cortesia.DesativarCupom();

            await _dbContext.SaveChangesAsync();
        }

        public async Task ApagarCupom(Cortesia cortesia) {
            _dbContext.Remove(cortesia);

            await _dbContext.SaveChangesAsync();
        }
    }
}

