using TicketsRavelli.Application.InputModels.Atletas;
using TicketsRavelli.Core.Entities.Atletas;

namespace TicketsRavelli.Application.Services.Interfaces;

public interface IAthleteService {
    Task<List<Atleta>> GetAllAthletesAsync();
    Task<bool> CheckAthleteExistsAsync(string cpf);
    Task<bool> CheckAthleteRegisteredByEmailAsync(string email);
    Task<bool> CheckAthleteRegisteredByRGAsync(string rg);
    Task<bool> VerificaAtletaExisteId(string id);
    Task<Atleta> GetAthleteByCpfAsync(string cpf);
    Task<Atleta> ConsultarAtletaPorCpfOuEmail(string cpfEmail);
    Task<Atleta> ConsultarAtletaPorId(string id);
    Task<Atleta> ConsultarAtletaPorEmail(string email);
    Task<Atleta> CadastrarAtleta<T>(T atletaInputModel);
    Task AtualizarAtleta(Atleta atleta, UpdateAthleteInputModel atletaInputModel);
    Task AtualizarCpfAtleta(Atleta atleta, string cpf);
    Task AtualizarCamisasAtletas(Atleta atleta, AtualizaCamisaInputModel atualizaCamisaInputModel);
    Task DeletarAtleta(Atleta atleta);
}