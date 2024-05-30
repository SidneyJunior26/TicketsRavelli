using TicketsRavelli.Core.Entities.Eventos;

namespace TicketsRavelli.Infrastructure.Persistence.Repositories.Interfaces;

public interface ISubscriptionRepository
{
    Task<List<Subscription>> QueryAllSubscriptionsAsync();
    Task<List<Subscription>> QueryAllSubscriptionsWithEventAsync();
    Task<Subscription> QuerySubscriptionByIdAsync(int id);
    Task<List<Subscription>> QueryUnpaidPixSubscriptionsAsync(int idEvento);
    Task<List<Subscription>> QueryUnpaidBilletSubscriptionsAsync(int idEvento);
    Task<List<Subscription>> QuerySubscriptionsForEventAsync(int idEvento);
    Task<List<Subscription>> QueryRegisteredSubscriptionsForEventAsync(int idEvento);
    Task<List<Subscription>> QueryNonEffectiveSubscriptionsForEventAsync(int idEvento);
    Task<Subscription?> QueryChargeIdAsync(int chargeId);
    Task<List<Subscription>> QueryAthleteSubscriptionsAsync(string cpfAtleta);
    Task<bool> QueryCheckIfAthleteRegisteredForEventAsync(string cpfAtleta, int idEvento);
    Task CreateAsync(Subscription inscricaoEvento);
    void UpdateSubscriptionState(Subscription inscricaoEvento);
    void DeleteAsync(Subscription inscricaoEvento);
    Task SaveChangesAsync();
}