using TicketsRavelli.Core.Entities.Atletas;

namespace TicketsRavelli.Infrastructure.Persistence.Repositories.Interfaces;

public interface IAtletaRepository {
    Task<List<Atleta>> ConsultarTodosAtletas();
    Task<bool> VerificaAtletaExisteCpf(string cpf);
    Task<bool> VerificaAtletaExisteEmail(string email);
    Task<Atleta> VerificaAtletaExisteRG(string rg);
    Task<bool> VerificaAtletaExisteId(string id);
    Task<Atleta> ConsultarAtletaPorCpf(string cpf);
    Task<Atleta> ConsultarAtletaPorCpfOuEmail(string cpfEmail);
    Task<Atleta> ConsultarAtletaPorId(string id);
    Task<Atleta> ConsultarAtletaPorEmail(string email);
    Task CadastrarAtleta(Atleta atleta);
    Task SalvarAlteracoesAsync();
    void DeletarAtleta(Atleta atleta);
    Task<bool> VerificaAtletaJaCadastrado(string cpf);
}

