using Microsoft.EntityFrameworkCore;
using TicketsRavelli.Application.Services.Interfaces;
using TicketsRavelli.Controllers.Regulamentos;
using TicketsRavelli.Core.Entities.Eventos;
using TicketsRavelli.Infra.Data;

namespace TicketsRavelli.Application.Services.Implementations
{
    public class RegulamentoService : IRegulamentoService {
        private readonly ApplicationDbContext _context;

        public RegulamentoService(ApplicationDbContext context) {
            _context = context;
        }

        public async Task AtualizarRegulamento(Regulamento regulamento, EditarRegulamentoInputModel editarRegulamentoInputModel) {
            regulamento.editarRegulamento(editarRegulamentoInputModel.regulamento1, editarRegulamentoInputModel.compromisso);

            await _context.SaveChangesAsync();
        }

        public async Task CadastrarRegulamento(NovoRegulamentoInputModel novoRegulamentoInputModel) {
            var novoRegulamento = new Regulamento(novoRegulamentoInputModel.idEvento, novoRegulamentoInputModel.regulamento1, novoRegulamentoInputModel.compromisso);

            await _context.AddAsync(novoRegulamento);
            await _context.SaveChangesAsync();
        }

        public async Task<Regulamento> ConsultarRegulamentoIdEvento(int idEvento) {
            return await _context.Regulamentos
                .SingleOrDefaultAsync(e => e.IdEvento == idEvento);
        }
    }
}

