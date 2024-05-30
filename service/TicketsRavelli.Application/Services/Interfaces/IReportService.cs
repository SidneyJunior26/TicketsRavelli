using TicketsRavelli.Application.ViewModels.Relatorios;

namespace TicketsRavelli.Application.Services.Interfaces
{
    public interface IReportService
    {
        Task<List<RelatorioInscricoesViewModel>> GetSubscriptionsReportAsync(int idEvento);
        Task<List<RelatorioInscricoesViewModel>> GetSubscriptionsCategoriesReportAsync(int idEvento);
        Task<List<RelatorioTotalInscritosCategoriaInpurModel>> GetTotalSubscriptionsByCategoriesReportAsync(int idEvento);
        Task<List<RelatorioTotalInscritosCategoriaInpurModel>> GetTotalEffectiveSubscriptionsAsync(int idEvento);
        Task<List<RelatorioTotalCamisasCategoriaInputModel>> GetTotalShirtsCategoriesAsync(int idEvento);
    }
}

