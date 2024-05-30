using TicketsRavelli.Application.Services.Interfaces;
using TicketsRavelli.Controllers.Regulamentos;
using TicketsRavelli.Core.Entities.Eventos;
using TicketsRavelli.Infrastructure.Persistence.Repositories.Interfaces;

namespace TicketsRavelli.Application.Services.Implementations
{
    public class RegulationService : IRegulationService
    {
        private readonly IRegulationRepository _regulationRepository;

        public RegulationService(IRegulationRepository regulationRepository)
        {
            _regulationRepository = regulationRepository;
        }

        public async Task UpdateAsync(Regulamento regulation, UpdateRegulationInputModel editarRegulamentoInputModel)
        {
            regulation.Update(editarRegulamentoInputModel.regulamento1, editarRegulamentoInputModel.compromisso);

            await _regulationRepository.SaveChangesAsync();
        }

        public async Task CreateAsync(NewRegulationInputModel newRegulationInputModel)
        {
            var regulation = new Regulamento(newRegulationInputModel.idEvento, newRegulationInputModel.regulamento1, newRegulationInputModel.compromisso);

            await _regulationRepository.CreateAsync(regulation);
            await _regulationRepository.SaveChangesAsync();
        }

        public async Task<Regulamento> GetByEventAsync(int idEvent)
        {
            return await _regulationRepository.QueryByEventAsync(idEvent);
        }
    }
}

