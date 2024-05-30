using TicketsRavelli.Application.InputModels.Inscricao;
using TicketsRavelli.Application.InputModels.Pagamentos.Pix;
using TicketsRavelli.Controllers.Inscricao;
using TicketsRavelli.Core.Entities.Eventos;
using TicketsRavelli.Core.Enums;

namespace TicketsRavelli.Application.Services.Interfaces
{
    public interface ISubscriptionService {
        Task<List<Subscription>> GetAllAsync();
        Task<List<Subscription>> GetAllSubscriptionsWithEventAsync();
        Task<Subscription> GetByIdAsync(int id);
        Task<List<Subscription>> GetUnpaidPixSubscriptionsAsync(int idEvento);
        Task<List<Subscription>> GetUnpaidBilletSubscriptionsAsync(int idEvento);
        Task<List<Subscription>> GetByEventAsync(int idEvento);
        Task<List<Subscription>> QueryRegisteredSubscriptionsForEventAsync(int idEvento);
        Task<List<Subscription>> GetNonEffectiveSubscriptionsByEventAsync(int idEvento);
        Task<Subscription?> GetChargeByTxIdAsync(int chargeId);
        Task<IEnumerable<SubscriptionAthleteViewModel>> GetAthleteSubscriptionsViewModel(string cpfAtleta);
        Task<List<Subscription>> GetAthleteSubscriptions(string cpfAtleta);
        Task<bool> CheckIfAthleteSubscribedByEventAsync(string cpfAtleta, int idEvento);
        Task<int> CreateAsync(SubscriptionInputModel inscricaoEventoInputModel);
        Task UpdateAsync(Subscription inscricaoEvento, SubscriptionInputModel inscricaoEventoInputModel);
        Task UpdateSubscriptionsCpfAthlete(List<Subscription> inscricaoEvento, string novoCpf);
        Task SubmitAsync(Subscription inscricaoEvento, decimal valorPago, MetodoPagamento metodoPagamento);
        Task SalvaInformacoesPagamentoPix(Subscription inscricaoEvento, NovoPagamentoPixInputModel novoPagamentoPixInputModel);
        Task ConfirmPaymentAsync(Subscription inscricao, decimal valorPago);
        Task DeleteAsync(Subscription inscricaoEvento);
        Task SaveBilletAsync(Subscription inscricao, SaveBilletInputModel salvarBoletoInscricaoInput);
        Task UpdatePaymentStatusAsync(Subscription inscricao, string status);
        Task UpdateTxIdAsync(Subscription inscricao, string txId, string status);
    }
}