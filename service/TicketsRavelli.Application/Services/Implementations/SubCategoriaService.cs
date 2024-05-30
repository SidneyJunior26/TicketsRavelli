using Microsoft.EntityFrameworkCore;
using TicketsRavelli.Application.Services.Interfaces;
using TicketsRavelli.Controllers.SubCategorias;
using TicketsRavelli.Core.Entities.Eventos;
using TicketsRavelli.Infra.Data;

namespace TicketsRavelli.Application.Services.Implementations
{
    public class SubCategoriaService : ISubCategoriaService {
        private readonly ApplicationDbContext _context;
        public SubCategoriaService(ApplicationDbContext context) {
            _context = context;
        }

        public async Task AtualizarCategoria(Subcategoria subcategoria, SubCategoriaInputModel subCategoriaInputModel) {
            subcategoria.EditarCategoria(subCategoriaInputModel.categoria, subCategoriaInputModel.descSubcategoria,
                subCategoriaInputModel.filtroSexo, subCategoriaInputModel.filtroDupla,
                subCategoriaInputModel.idadeDe, subCategoriaInputModel.idadeAte,
                subCategoriaInputModel.aviso, subCategoriaInputModel.ativo);

            await _context.SaveChangesAsync();
        }

        public async Task<int> CadastrarCategoria(SubCategoriaInputModel subCategoriaInputModel) {
            var novaSubCategoria = new Subcategoria(subCategoriaInputModel.idEvento,
                subCategoriaInputModel.categoria, subCategoriaInputModel.descSubcategoria,
                subCategoriaInputModel.filtroSexo, subCategoriaInputModel.filtroDupla,
                subCategoriaInputModel.idadeDe, subCategoriaInputModel.idadeAte,
                subCategoriaInputModel.aviso, subCategoriaInputModel.ativo);

            await _context.Subcategoria.AddAsync(novaSubCategoria);
            await _context.SaveChangesAsync();

            return novaSubCategoria.Id;
        }

        public async Task<Subcategoria> ConsultarCategoriaPeloId(int id) {
            return await _context.Subcategoria
                .SingleOrDefaultAsync(s => s.Id == id);
        }

        public async Task<List<Subcategoria>> ConsultarCategorias() {
            return await _context.Subcategoria
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Subcategoria>> ConsultarCategoriasEvento(int idEvento) {
            return await _context.Subcategoria
                .AsNoTracking()
                .Where(s => s.IdEvento == idEvento).ToListAsync();
        }

        public async Task<List<Subcategoria>> ConsultarCategoriasFiltrado(int idEvento, int categoria, int idade, int sexo) {
            return await _context.Subcategoria
                .AsNoTracking()
                .Where(s => s.IdEvento == idEvento
                            && s.Categoria == categoria
                            && s.IdadeAte >= idade
                            && s.IdadeDe <= idade
                            && (s.FiltroSexo == sexo || s.FiltroSexo == 0)
                            && s.Ativo == true).ToListAsync();
        }

        public async Task DeletarCategoria(Subcategoria subcategoria) {
            _context.Subcategoria.Remove(subcategoria);
            await _context.SaveChangesAsync();
        }
    }
}

