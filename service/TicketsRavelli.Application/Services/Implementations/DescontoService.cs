using Microsoft.EntityFrameworkCore;
using TicketsRavelli.Application.InputModels.Eventos;
using TicketsRavelli.Application.Services.Interfaces;
using TicketsRavelli.Core.Entities.Descontos;
using TicketsRavelli.Infra.Data;

namespace TicketsRavelli.Application.Services.Implementations {
    public class DescontoService : IDescontoService {
        private readonly ApplicationDbContext _dbContext;

        public DescontoService(ApplicationDbContext dbContext) {
            _dbContext = dbContext;
        }

        public decimal AplicarDesconto(Desconto cupomDesconto, decimal valor) {
            decimal valorDesconto = (Convert.ToDecimal(cupomDesconto.PorcDesconto) / Convert.ToDecimal(100)) * Convert.ToInt16(valor);

            decimal valorFinal = valor - valorDesconto;

            return valorFinal;
        }

        public async Task AtivarCupom(Desconto desconto) {
            desconto.AtivarCupom();

            await _dbContext.SaveChangesAsync();
        }

        public async Task AtualizarDesconto(Desconto desconto, AtualizarCupomInputModel atualizarCupomInputModel) {
            desconto.AtualizarDesconto(desconto.Cupom, desconto.PorcDesconto);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<int> CadastrarDesconto(NovoDescontoInputModel novoDescontoInputModel) {
            var desconto = new Desconto(novoDescontoInputModel.idEvento,
                novoDescontoInputModel.cupom,
                novoDescontoInputModel.porcentagem,
                novoDescontoInputModel.ativo);

            await _dbContext.Descontos.AddAsync(desconto);
            await _dbContext.SaveChangesAsync();

            return desconto.Id;
        }

        public async Task<Desconto> ConsultarDescontoId(int id) {
            return await _dbContext.Descontos.SingleOrDefaultAsync(d => d.Id == id);
        }

        public async Task<List<Desconto>> ConsultarDescontosEvento(int idEvento) {
            return await _dbContext.Descontos.AsNoTracking().Where(d => d.IdEvento == idEvento).ToListAsync();
        }

        public async Task DeletarCupom(Desconto desconto) {
            _dbContext.Descontos.Remove(desconto);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DesativarCupom(Desconto desconto) {
            desconto.DesativarCupom();

            await _dbContext.SaveChangesAsync();
        }

        public async Task<Desconto> ValidarDesconto(string cupom, int idEvento) {
            return await _dbContext.Descontos
                .AsNoTracking()
                .SingleOrDefaultAsync(d => d.Cupom == cupom &&
                d.IdEvento == idEvento &&
                d.Ativo == 1);
        }
    }
}

