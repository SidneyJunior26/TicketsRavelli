using TicketsRavelli.Application.InputModels.Eventos;
using TicketsRavelli.Core.Entities.Descontos;

namespace TicketsRavelli.Application.Services.Interfaces;

public interface IDiscountService
{
    Task<int> CreateAsync(NewCoupomInputModel novoDescontoInputModel);
    Task Update(Desconto desconto, UpdateCoupomInputModel atualizarCupomInputModel);
    Task<Desconto> GetByIdAsync(int id);
    Task<List<Desconto>> GetByEventAsync(int idEvento);
    Task<Desconto> ValidateAsync(string cupom, int idEvento);
    Task DisableAsync(Desconto desconto);
    Task ActivateAsync(Desconto desconto);
    Task DeleteAsync(Desconto desconto);
    decimal GetFinalValue(Desconto desconto, decimal valor);
}

