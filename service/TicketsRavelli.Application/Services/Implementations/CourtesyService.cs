using Microsoft.EntityFrameworkCore;
using TicketsRavelli.Application.Services.Interfaces;
using TicketsRavelli.Core.Entities.Cortesias;
using TicketsRavelli.Infra.Data;
using TicketsRavelli.Infrastructure.Persistence.Repositories.Interfaces;

namespace TicketsRavelli.Application.Services.Implementations
{
    public class CourtesyService : ICourtesyService
    {
        private readonly ICourtesyRepository _courtesyRepository;

        public CourtesyService(ICourtesyRepository courtesyRepository)
        {
            _courtesyRepository = courtesyRepository;
        }

        public async Task UpdateStatusCoupomAsync(Courtesy cortesia)
        {
            cortesia.UpdateStatus();

            await _courtesyRepository.SaveChangesAsync();
        }

        public async Task<string> CreateCouponEventAsync(int idEvento)
        {
            var guid = Guid.NewGuid().ToString();

            var cupom = guid.Substring(0, guid.IndexOf("-"));

            var courtesy = new Courtesy(idEvento, cupom.ToString());

            await _courtesyRepository.CreateCouponEventAsync(courtesy);
            await _courtesyRepository.SaveChangesAsync();

            return cupom;
        }

        public async Task<Courtesy> GetCoupomCourtesyAsync(string coupom)
        {
            return await _courtesyRepository.QueryCoupomCourtesyAsync(coupom);
        }

        public async Task<Courtesy?> GetActiveCourtesyCouponEventAsync(int idEvent, string coupom)
        {
            return await _courtesyRepository.QueryActiveCourtesyCouponEventAsync(idEvent, coupom);
        }

        public async Task<List<Courtesy>> GetCouponsEventAsync(int idEvent)
        {
            return await _courtesyRepository.QueryCouponsEventAsync(idEvent);
        }

        public async Task DisableCoupomAsync(Courtesy courtesy)
        {
            courtesy.Disable();

            await _courtesyRepository.SaveChangesAsync();
        }

        public async Task DeleteCoupomAsync(Courtesy courtesy)
        {
            _courtesyRepository.DeleteCoupom(courtesy);

            await _courtesyRepository.SaveChangesAsync();
        }
    }
}