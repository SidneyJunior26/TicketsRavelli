using Microsoft.EntityFrameworkCore;
using TicketsRavelli.Core.Entities.Atletas;
using TicketsRavelli.Infra.Data;
using TicketsRavelli.Infrastructure.Persistence.Repositories.Interfaces;

namespace TicketsRavelli.Infrastructure.Persistence.Repositories.Implementations {
    public class AtletaRepository : IAtletaRepository {
        private readonly ApplicationDbContext _context;

        public AtletaRepository(ApplicationDbContext context) {
            _context = context;
        }

        public async Task CadastrarAtleta(Atleta atleta) {
            await _context.Atletas.AddAsync(atleta);
        }

        public async Task<Atleta> ConsultarAtletaPorCpf(string cpf) {
            return await _context.Atletas
                .Include(a => a.RegistroMedico)
                .SingleOrDefaultAsync(a => a.Cpf == cpf);
        }

        public async Task<Atleta> ConsultarAtletaPorCpfOuEmail(string cpfEmail) {
            return await _context.Atletas
                .SingleOrDefaultAsync(a => a.Cpf == cpfEmail || a.Email == cpfEmail);
        }

        public async Task<Atleta> ConsultarAtletaPorEmail(string email) {
            return await _context.Atletas
                .Include(a => a.RegistroMedico)
                .SingleOrDefaultAsync(a => a.Email == email);
        }

        public async Task<Atleta> ConsultarAtletaPorId(string id) {
            return await _context.Atletas
                .Include(a => a.RegistroMedico)
                .SingleOrDefaultAsync(a => a.Id == id);
        }

        public async Task<List<Atleta>> ConsultarTodosAtletas() {
            return await _context.Atletas.AsNoTracking().OrderBy(a => a.Nome.Trim()).ToListAsync();
        }

        public void DeletarAtleta(Atleta atleta) {
            _context.Atletas.Remove(atleta);
        }

        public async Task SalvarAlteracoesAsync() {
            await _context.SaveChangesAsync();
        }

        public async Task<bool> VerificaAtletaExisteCpf(string cpf) {
            return await _context.Atletas
                .AsNoTracking()
                .AnyAsync(a => a.Cpf == cpf);
        }

        public async Task<bool> VerificaAtletaExisteEmail(string email) {
            return await _context.Atletas
                .AsNoTracking()
                .AnyAsync(a => a.Email == email);
        }

        public async Task<bool> VerificaAtletaExisteId(string id) {
            return await _context.Atletas
                .AsNoTracking()
                .AnyAsync(a => a.Id == id);
        }

        public async Task<Atleta> VerificaAtletaExisteRG(string rg) {
            return await _context.Atletas
                .AsNoTracking()
                .SingleOrDefaultAsync(a => a.Rg == rg);
        }

        public async Task<bool> VerificaAtletaJaCadastrado(string cpf) {
            return await _context.Atletas
                    .AsNoTracking()
                    .AnyAsync(a => a.Cpf == cpf);
        }
    }
}

