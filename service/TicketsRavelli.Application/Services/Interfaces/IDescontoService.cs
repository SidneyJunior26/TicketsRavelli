using TicketsRavelli.Application.InputModels.Eventos;
using TicketsRavelli.Core.Entities.Descontos;

namespace TicketsRavelli.Application.Services.Interfaces;

public interface IDescontoService
{
    Task<int> CadastrarDesconto(NovoDescontoInputModel novoDescontoInputModel);
    Task AtualizarDesconto(Desconto desconto, AtualizarCupomInputModel atualizarCupomInputModel);
    Task<Desconto> ConsultarDescontoId(int id);
    Task<List<Desconto>> ConsultarDescontosEvento(int idEvento);
    Task<Desconto> ValidarDesconto(string cupom, int idEvento);
    Task DesativarCupom(Desconto desconto);
    Task AtivarCupom(Desconto desconto);
    Task DeletarCupom(Desconto desconto);
    decimal AplicarDesconto(Desconto desconto, decimal valor);
}

