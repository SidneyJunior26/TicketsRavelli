using TicketsRavelli.Application.InputModels.Inscricao;
using TicketsRavelli.Application.InputModels.Pagamentos.Pix;
using TicketsRavelli.Application.Services.Interfaces;
using TicketsRavelli.Controllers.Inscricao;
using TicketsRavelli.Core.Enums;
using TicketsRavelli.Infrastructure.Persistence.Repositories.Interfaces;

namespace TicketsRavelli.Application.Services.Implementations;

public class SubscriptionService : ISubscriptionService
{
    private readonly ISubscriptionRepository _subscriptionRepository;

    public SubscriptionService(ISubscriptionRepository inscricaoRepository)
    {
        _subscriptionRepository = inscricaoRepository;
    }

    public async Task UpdateAsync(Core.Entities.Eventos.Subscription subscription, SubscriptionInputModel subscriptionInputModel)
    {
        subscription.Update(subscriptionInputModel.idSubcategoria, subscriptionInputModel.equipe,
            subscriptionInputModel.dupla, subscriptionInputModel.quarteto);

        await _subscriptionRepository.SaveChangesAsync();
    }

    public async Task<int> CreateAsync(SubscriptionInputModel subscriptionInputModel)
    {
        var newSubscription = new Core.Entities.Eventos.Subscription(subscriptionInputModel.idEvento, subscriptionInputModel.cpfAtleta,
            subscriptionInputModel.idSubcategoria, subscriptionInputModel.equipe, subscriptionInputModel.dupla,
            subscriptionInputModel.quarteto, subscriptionInputModel.numeral, subscriptionInputModel.pacote,
            subscriptionInputModel.aceiteRegulamento, subscriptionInputModel.afiliadoId, subscriptionInputModel.pago);

        await _subscriptionRepository.CreateAsync(newSubscription);
        await _subscriptionRepository.SaveChangesAsync();

        return newSubscription.Id;
    }

    public async Task ConfirmPaymentAsync(Core.Entities.Eventos.Subscription subscription, decimal amountPaid)
    {
        subscription.ConfirmPayment(amountPaid);

        await _subscriptionRepository.SaveChangesAsync();
    }

    public async Task<List<Core.Entities.Eventos.Subscription>> GetByEventAsync(int idEvent)
    {
        return await _subscriptionRepository.QuerySubscriptionsForEventAsync(idEvent);
    }

    public async Task<List<Core.Entities.Eventos.Subscription>> QueryRegisteredSubscriptionsForEventAsync(int idEvent)
    {
        return await _subscriptionRepository.QueryRegisteredSubscriptionsForEventAsync(idEvent);
    }

    public async Task<Core.Entities.Eventos.Subscription> GetByIdAsync(int idInscricao)
    {
        return await _subscriptionRepository.QuerySubscriptionByIdAsync(idInscricao);
    }

    public async Task<List<Core.Entities.Eventos.Subscription>> GetAllAsync()
    {
        return await _subscriptionRepository.QueryAllSubscriptionsAsync();
    }

    public async Task<IEnumerable<SubscriptionAthleteViewModel>> GetAthleteSubscriptionsViewModel(string cpfAtleta)
    {
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

    public async Task<List<Core.Entities.Eventos.Subscription>> GetAthleteSubscriptions(string cpfAtleta)
    {
        var subscriptions = await _subscriptionRepository.QueryAthleteSubscriptionsAsync(cpfAtleta);

        return subscriptions;
    }

    public async Task<Core.Entities.Eventos.Subscription> GetChargeByTxIdAsync(int chargeId)
    {
        return await _subscriptionRepository.QueryChargeIdAsync(chargeId);
    }

    public async Task DeleteAsync(Core.Entities.Eventos.Subscription inscricaoEvento)
    {
        _subscriptionRepository.DeleteAsync(inscricaoEvento);

        await _subscriptionRepository.SaveChangesAsync();
    }

    public async Task SalvaInformacoesPagamentoPix(Core.Entities.Eventos.Subscription inscricaoEvento, NovoPagamentoPixInputModel novoPagamentoPixInputModel)
    {
        inscricaoEvento.UpdatePaymentPixInfo(novoPagamentoPixInputModel.tempoExpiracao,
            novoPagamentoPixInputModel.txId,
            novoPagamentoPixInputModel.qrCode,
            novoPagamentoPixInputModel.status);

        await _subscriptionRepository.SaveChangesAsync();
    }

    public async Task<List<Core.Entities.Eventos.Subscription>> GetNonEffectiveSubscriptionsByEventAsync(int idEvento)
    {
        return await _subscriptionRepository.QueryNonEffectiveSubscriptionsForEventAsync(idEvento);
    }

    public async Task SubmitAsync(Core.Entities.Eventos.Subscription inscricaoEvento, decimal valorPago, MetodoPagamento metodoPagamento)
    {
        inscricaoEvento.Submit(valorPago, metodoPagamento);

        await _subscriptionRepository.SaveChangesAsync();
    }

    public async Task SaveBilletAsync(Core.Entities.Eventos.Subscription inscricao, SaveBilletInputModel salvarBoletoInscricaoInput)
    {
        inscricao.UpdateBilletData(salvarBoletoInscricaoInput.gnExpiracao,
            salvarBoletoInscricaoInput.gnChargeId,
            salvarBoletoInscricaoInput.gnTotal,
            salvarBoletoInscricaoInput.gnLink,
            salvarBoletoInscricaoInput.gnBarCode,
            salvarBoletoInscricaoInput.gnStatus);

        await _subscriptionRepository.SaveChangesAsync();
    }

    public async Task<bool> CheckIfAthleteSubscribedByEventAsync(string cpfAthlete, int idEvent)
    {
        return await _subscriptionRepository.QueryCheckIfAthleteRegisteredForEventAsync(cpfAthlete, idEvent);
    }

    public async Task UpdatePaymentStatusAsync(Core.Entities.Eventos.Subscription subscription, string status)
    {
        subscription.UpdatePaymentStatus(status);

        await _subscriptionRepository.SaveChangesAsync();
    }

    public async Task UpdateTxIdAsync(Core.Entities.Eventos.Subscription inscricao, string txId, string status)
    {
        inscricao.UpdateTxId(txId, status);

        await _subscriptionRepository.SaveChangesAsync();
    }

    public async Task UpdateSubscriptionsCpfAthlete(List<Core.Entities.Eventos.Subscription> subscriptions, string newCpf)
    {
        foreach (var subscription in subscriptions)
        {
            _subscriptionRepository.UpdateSubscriptionState(subscription);
            subscription.UpdateCpf(newCpf);
        }

        await _subscriptionRepository.SaveChangesAsync();
    }

    public async Task<List<Core.Entities.Eventos.Subscription>> GetUnpaidPixSubscriptionsAsync(int idEvent)
    {
        return await _subscriptionRepository.QueryUnpaidPixSubscriptionsAsync(idEvent);
    }

    public async Task<List<Core.Entities.Eventos.Subscription>> GetUnpaidBilletSubscriptionsAsync(int idEvent)
    {
        return await _subscriptionRepository.QueryUnpaidBilletSubscriptionsAsync(idEvent);
    }

    public async Task<List<Core.Entities.Eventos.Subscription>> GetAllSubscriptionsWithEventAsync()
    {
        return await _subscriptionRepository.QueryAllSubscriptionsWithEventAsync();
    }
}