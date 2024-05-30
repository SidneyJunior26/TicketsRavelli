using TicketsRavelli.Core.Entities.Athletes;

namespace TicketsRavelli.Infrastructure.Persistence.Repositories.Interfaces;

public interface IMedicalDataRepository
{
    Task SaveChangesAsync();
    Task CreateAsync(RegistroMedico medicalData);
    Task<RegistroMedico> QueryByAthleteAsync(string idAthlete);
}