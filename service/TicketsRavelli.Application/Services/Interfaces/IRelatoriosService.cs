using TicketsRavelli.Application.ViewModels.Relatorios;

namespace TicketsRavelli.Application.Services.Interfaces
{
    public interface IRelatoriosService
    {
        Task<List<RelatorioInscricoesViewModel>> RelatorioInscricaoId(int idEvento);
        Task<List<RelatorioInscricoesViewModel>> RelatorioInscricaoCategoriaId(int idEvento);
        Task<List<RelatorioTotalInscritosCategoriaInpurModel>> RelatorioTotalInscritosPorCategoria(int idEvento);
        Task<List<RelatorioTotalInscritosCategoriaInpurModel>> RelatorioTotalInscritosEfetivados(int idEvento);
        Task<List<RelatorioTotalCamisasCategoriaInputModel>> RelatorioTotalCamisasCategoria(int idEvento);
    }
}

