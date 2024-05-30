using TicketsRavelli.Application.InputModels.Inscricao;
using TicketsRavelli.Application.InputModels.Pagamentos.Pix;
using TicketsRavelli.Application.Services.Interfaces;
using TicketsRavelli.Controllers.Inscricao;
using TicketsRavelli.Core.Entities.Eventos;
using TicketsRavelli.Core.Enums;
using TicketsRavelli.Infrastructure.Persistence.Repositories.Interfaces;

namespace TicketsRavelli.Application.Services.Implementations {
    public class InscricaoService : ISubscriptionService {

        private readonly ISubscriptionRepository _subscriptionRepository;

        public InscricaoService(ISubscriptionRepository inscricaoRepository) {
            _subscriptionRepository = inscricaoRepository;
        }

        public async Task UpdateSubscriptionAsync(Subscription subscription, SubscriptionInputModel subscriptionInputModel) {
            subscription.UpdateSubscription(subscriptionInputModel.idSubcategoria, subscriptionInputModel.equipe,
                subscriptionInputModel.dupla, subscriptionInputModel.quarteto);

            await _subscriptionRepository.SaveChangesAsync();
        }

        public async Task<int> CreateSubscriptionAsync(SubscriptionInputModel subscriptionInputModel) {
            var newSubscription = new Subscription(subscriptionInputModel.idEvento, subscriptionInputModel.cpfAtleta,
            subscriptionInputModel.idSubcategoria, subscriptionInputModel.equipe, subscriptionInputModel.dupla,
            subscriptionInputModel.quarteto, subscriptionInputModel.numeral, subscriptionInputModel.pacote,
            subscriptionInputModel.aceiteRegulamento, subscriptionInputModel.afiliadoId, subscriptionInputModel.pago);

            await _subscriptionRepository.CreateSubscriptionAsync(newSubscription);
            await _subscriptionRepository.SaveChangesAsync();

            return newSubscription.Id;
        }

        public async Task ConfirmPaymentAsync(Subscription subscription, decimal amountPaid) {
            subscription.ConfirmPayment(amountPaid);

            await _subscriptionRepository.SaveChangesAsync();
        }

        public async Task<List<Subscription>> GetSubscriptionsByEventAsync(int idEvent) {
            return await _subscriptionRepository.QuerySubscriptionsForEventAsync(idEvent);
        }

        public async Task<List<Subscription>> QueryRegisteredSubscriptionsForEventAsync(int idEvent) {
            return await _subscriptionRepository.QueryRegisteredSubscriptionsForEventAsync(idEvent);
        }

        public async Task<Subscription> GetSubscriptionByIdAsync(int idInscricao) {
            return await _subscriptionRepository.QuerySubscriptionByIdAsync(idInscricao);
        }

        public async Task<List<Subscription>> GetAllSubscriptionsAsync() {
            return await _subscriptionRepository.QueryAllSubscriptionsAsync();
        }

        public async Task<IEnumerable<SubscriptionAthleteViewModel>> GetAthleteSubscriptionsViewModel(string cpfAtleta) {
            var subscriptions = await _subscriptionRepository.QueryAthleteSubscriptionsAsync(cpfAtleta);

            var subscriptionAthleteViewModel = subscriptions.Select(i => new SubscriptionAthleteViewModel(
            i.Id, i.Evento.Id, i.Evento.Nome, i.Subcategoria.DescSubcategoria, i.Pago,
            i.Evento.DataFimInscricao >= DateTime.Today,
            i.GnLink != null && i.GnExpireAt != null && i.GnExpireAt.Value.Date >= DateTime.Today.Date ?
                i.GnLink : "",
            i.Evento.AtivaAlteracaoInscricao == 1,
            i.Pacote));

            return subscriptionAthleteViewModel;
        }

        public async Task<List<Subscription>> GetAthleteSubscriptions(string cpfAtleta) {
            var subscriptions = await _subscriptionRepository.QueryAthleteSubscriptionsAsync(cpfAtleta);

            return subscriptions;
        }

        public async Task<Subscription> GetChargeIdAsync(int chargeId) {
            return await _subscriptionRepository.QueryChargeIdAsync(chargeId);
        }

        public async Task DeleteSubscriptionAsync(Subscription inscricaoEvento) {
            _subscriptionRepository.DeleteSubscriptionAsync(inscricaoEvento);
            await _subscriptionRepository.SaveChangesAsync();
        }

        public async Task SalvaInformacoesPagamentoPix(Subscription inscricaoEvento, NovoPagamentoPixInputModel novoPagamentoPixInputModel) {
            inscricaoEvento.UpdatePaymentPixInfo(novoPagamentoPixInputModel.tempoExpiracao,
                novoPagamentoPixInputModel.txId,
                novoPagamentoPixInputModel.qrCode,
                novoPagamentoPixInputModel.status);

            await _subscriptionRepository.SaveChangesAsync();
        }

        public async Task<List<Subscription>> GetNonEffectiveSubscriptionsByEventAsync(int idEvento) {
            return await _subscriptionRepository.QueryNonEffectiveSubscriptionsForEventAsync(idEvento);
        }

        public async Task SubmitSubscriptionAsync(Subscription inscricaoEvento, decimal valorPago, MetodoPagamento metodoPagamento) {
            inscricaoEvento.SubmitSubscription(valorPago, metodoPagamento);

            await _subscriptionRepository.SaveChangesAsync();
        }

        public async Task SaveBilletDataAsync(Subscription inscricao, SalvarBoletoInscricaoInputModel salvarBoletoInscricaoInput) {
            inscricao.UpdateBilletData(salvarBoletoInscricaoInput.gnExpiracao,
                salvarBoletoInscricaoInput.gnChargeId,
                salvarBoletoInscricaoInput.gnTotal,
                salvarBoletoInscricaoInput.gnLink,
                salvarBoletoInscricaoInput.gnBarCode,
                salvarBoletoInscricaoInput.gnStatus);

            await _subscriptionRepository.SaveChangesAsync();
        }

        public async Task<bool> CheckIfAthleteSubscribedByEventAsync(string cpfAthlete, int idEvent) {
            return await _subscriptionRepository.QueryCheckIfAthleteRegisteredForEventAsync(cpfAthlete, idEvent);
        }

        public async Task UpdatePaymentStatus(Subscription subscription, string status) {
            subscription.UpdatePaymentStatus(status);

            await _subscriptionRepository.SaveChangesAsync();
        }

        public async Task UpdateTxId(Subscription inscricao, string txId, string status) {
            inscricao.UpdateTxId(txId, status);

            await _subscriptionRepository.SaveChangesAsync();
        }

        public async Task UpdateSubscriptionsCpfAthlete(List<Subscription> subscriptions, string newCpf) {
            foreach (var subscription in subscriptions) {
                _subscriptionRepository.UpdateSubscriptionState(subscription);
                subscription.UpdateCpf(newCpf);
            }

            await _subscriptionRepository.SaveChangesAsync();
        }

        public async Task<List<Subscription>> GetUnpaidPixSubscriptionsAsync(int idEvent) {
            return await _subscriptionRepository.QueryUnpaidPixSubscriptionsAsync(idEvent);
        }

        public async Task<List<Subscription>> GetUnpaidBilletSubscriptionsAsync(int idEvent) {
            return await _subscriptionRepository.QueryUnpaidBilletSubscriptionsAsync(idEvent);
        }

        public async Task<List<Subscription>> GetAllSubscriptionsWithEventAsync() {
            return await _subscriptionRepository.QueryAllSubscriptionsWithEventAsync();
        }
    }
}