using TicketsRavelli.Core.Entities.Cortesias;

namespace TicketsRavelli.Infrastructure.Persistence.Repositories.Interfaces;

public interface ICourtesyRepository
{
    Task<Courtesy?> QueryActiveCourtesyCouponEventAsync(int idEvent, string coupom);
    Task<List<Courtesy>> QueryCouponsEventAsync(int idEvent);
    Task<Courtesy> QueryCoupomCourtesyAsync(string coupom);
    Task CreateCouponEventAsync(Courtesy courtesy);
    void DeleteCoupom(Courtesy courtesy);
    Task SaveChangesAsync();
}