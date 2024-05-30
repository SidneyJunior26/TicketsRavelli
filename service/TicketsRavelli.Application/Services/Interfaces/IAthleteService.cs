using TicketsRavelli.Application.InputModels.Atletas;
using TicketsRavelli.Core.Entities.Athletes;

namespace TicketsRavelli.Application.Services.Interfaces;

public interface IAthleteService {
    Task<List<Athlete>> GetAllAsync();
    Task<bool> CheckAthleteExistsByCpfAsync(string cpf);
    Task<bool> CheckAthleteRegisteredByEmailAsync(string email);
    Task<bool> CheckAthleteRegisteredByRGAsync(string rg);
    Task<bool> CheckAthleteExistsByIdAsync(string id);
    Task<Athlete> GetByCpfAsync(string cpf);
    Task<Athlete> GetByCpfOrEmailAsync(string cpfEmail);
    Task<Athlete> GetByIdAsync(string id);
    Task<Athlete> GetByEmailAsync(string email);
    Task<Athlete> CreateAsync<T>(T atletaInputModel);
    Task UpdateAsync(Athlete atleta, UpdateAthleteInputModel atletaInputModel);
    Task UpdateCpfAsync(Athlete atleta, string cpf);
    Task UpdateShirtAsync(Athlete atleta, UpdateShirtInputModel atualizaCamisaInputModel);
    Task DeleteAsync(Athlete atleta);
}