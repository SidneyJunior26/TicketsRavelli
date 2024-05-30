using TicketsRavelli.Controllers.SubCategorias;
using TicketsRavelli.Core.Entities.Eventos;

namespace TicketsRavelli.Application.Services.Interfaces
{
    public interface ISubCategoriaService {
        Task<List<Subcategoria>> ConsultarCategorias();
        Task<List<Subcategoria>> ConsultarCategoriasEvento(int idEvento);
        Task<Subcategoria> ConsultarCategoriaPeloId(int id);
        Task<List<Subcategoria>> ConsultarCategoriasFiltrado(int idEvento, int categoria, int idade, int sexo);
        Task<int> CadastrarCategoria(SubCategoriaInputModel subCategoriaInputModel);
        Task AtualizarCategoria(Subcategoria subcategoria, SubCategoriaInputModel subCategoriaInputModel);
        Task DeletarCategoria(Subcategoria subcategoria);
    }
}

