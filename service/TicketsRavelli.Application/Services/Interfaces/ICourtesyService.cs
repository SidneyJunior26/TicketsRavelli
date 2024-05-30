using TicketsRavelli.Core.Entities.Cortesias;

namespace TicketsRavelli.Application.Services.Interfaces;

public interface ICourtesyService
{
    Task<Courtesy?> GetActiveCourtesyCouponEventAsync(int idEvent, string coupom);
    Task<List<Courtesy>> GetCouponsEventAsync(int idEvent);
    Task<Courtesy> GetCoupomCourtesyAsync(string coupom);
    Task UpdateStatusCoupomAsync(Courtesy courtesy);
    Task DisableCoupomAsync(Courtesy courtesy);
    Task<string> CreateCouponEventAsync(int idEvent);
    Task DeleteCoupomAsync(Courtesy courtesy);
}