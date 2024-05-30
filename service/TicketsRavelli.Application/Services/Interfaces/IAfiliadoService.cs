using TicketsRavelli.Application.InputModels;
using TicketsRavelli.Core.Entities.Afiliados;

namespace TicketsRavelli.Application.Services.Interfaces;

public interface IAfiliadoService {
    Task CadastrarAfiliado(AfiliadoInputModel afiliado);
    Task AtualizarAfiliado(Afiliado afiliado, AfiliadoInputModel afiliadoInput);
    Task<List<Afiliado>> ConsultarAfiliados();
    Task<Afiliado> ConsultarAfiliadoId(string id);
    Task<Afiliado> ConsultarAfiliadoCpf(string cpf);
    Task<List<Afiliado>> ConsultarAfiliadosEvento(int idEvento);
    Task DeletarAfiliado(Afiliado afiliado);
}

