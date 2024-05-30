using TicketsRavelli.Controllers.RegistrosMedicos;
using TicketsRavelli.Core.Entities.Athletes;

namespace TicketsRavelli.Application.Services.Interfaces
{
    public interface IMedicalDataService {
        Task<RegistroMedico> GetByAthleteAsync(string idAtleta);
        Task CreateAsync(MedicalDataInputModel registroMedicoInputModel);
        Task UpdateAsync(RegistroMedico registroMedico, MedicalDataInputModel registroMedicoInputModel);
    }
}

