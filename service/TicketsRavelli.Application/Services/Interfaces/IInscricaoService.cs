using TicketsRavelli.Application.InputModels.Inscricao;
using TicketsRavelli.Application.InputModels.Pagamentos.Pix;
using TicketsRavelli.Controllers.Inscricao;
using TicketsRavelli.Core.Entities.Eventos;
using TicketsRavelli.Core.Enums;

namespace TicketsRavelli.Application.Services.Interfaces
{
    public interface ISubscriptionService {
        Task<List<Subscription>> GetAllSubscriptionsAsync();
        Task<List<Subscription>> GetAllSubscriptionsWithEventAsync();
        Task<Subscription> GetSubscriptionByIdAsync(int id);
        Task<List<Subscription>> GetUnpaidPixSubscriptionsAsync(int idEvento);
        Task<List<Subscription>> GetUnpaidBilletSubscriptionsAsync(int idEvento);
        Task<List<Subscription>> GetSubscriptionsByEventAsync(int idEvento);
        Task<List<Subscription>> QueryRegisteredSubscriptionsForEventAsync(int idEvento);
        Task<List<Subscription>> GetNonEffectiveSubscriptionsByEventAsync(int idEvento);
        Task<Subscription?> GetChargeIdAsync(int chargeId);
        Task<IEnumerable<SubscriptionAthleteViewModel>> GetAthleteSubscriptionsViewModel(string cpfAtleta);
        Task<List<Subscription>> GetAthleteSubscriptions(string cpfAtleta);
        Task<bool> CheckIfAthleteSubscribedByEventAsync(string cpfAtleta, int idEvento);
        Task<int> CreateSubscriptionAsync(SubscriptionInputModel inscricaoEventoInputModel);
        Task UpdateSubscriptionAsync(Subscription inscricaoEvento, SubscriptionInputModel inscricaoEventoInputModel);
        Task UpdateSubscriptionsCpfAthlete(List<Subscription> inscricaoEvento, string novoCpf);
        Task SubmitSubscriptionAsync(Subscription inscricaoEvento, decimal valorPago, MetodoPagamento metodoPagamento);
        Task SalvaInformacoesPagamentoPix(Subscription inscricaoEvento, NovoPagamentoPixInputModel novoPagamentoPixInputModel);
        Task ConfirmPaymentAsync(Subscription inscricao, decimal valorPago);
        Task DeleteSubscriptionAsync(Subscription inscricaoEvento);
        Task SaveBilletDataAsync(Subscription inscricao, SalvarBoletoInscricaoInputModel salvarBoletoInscricaoInput);
        Task UpdatePaymentStatus(Subscription inscricao, string status);
        Task UpdateTxId(Subscription inscricao, string txId, string status);
    }
}