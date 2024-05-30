using TicketsRavelli.Core.Entities.Athletes;

namespace TicketsRavelli.Infrastructure.Persistence.Repositories.Interfaces;

public interface IAthleteRepository {
    Task UpdateCpfAsync(Athlete atlhete, string cpf);
    Task<List<Athlete>> QueryAllAsync();
    Task<bool> AthleteExistsByCPF(string cpf);
    Task<bool> AthleteExistsByEmail(string email);
    Task<Athlete> QueryAthleteByRG(string rg);
    Task<bool> AthleteExistsById(string id);
    Task<Athlete> QueryByCPF(string cpf);
    Task<Athlete> QueryAthleteByEmailOrCPF(string cpfEmail);
    Task<Athlete> QueryById(string id);
    Task<Athlete> QueryByEmail(string email);
    Task CreateAsync(Athlete atleta);
    Task SaveChangesAsync();
    void Delete(Athlete atleta);
}