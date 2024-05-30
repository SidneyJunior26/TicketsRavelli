using TicketsRavelli.Application.InputModels.Eventos;
using TicketsRavelli.Application.Services.Interfaces;
using TicketsRavelli.Core.Entities.Descontos;
using TicketsRavelli.Infrastructure.Persistence.Repositories.Interfaces;

namespace TicketsRavelli.Application.Services.Implementations;

public class DiscountService : IDiscountService
{
    private readonly IDiscountRepository _discountRepository;

    public DiscountService(IDiscountRepository discountRepository)
    {
        _discountRepository = discountRepository;
    }

    public decimal GetFinalValue(Desconto cupomDesconto, decimal value)
    {
        decimal discountValue = (Convert.ToDecimal(cupomDesconto.PorcDesconto) / Convert.ToDecimal(100)) * Convert.ToInt16(value);

        decimal finalValue = value - discountValue;

        return finalValue;
    }

    public async Task ActivateAsync(Desconto desconto)
    {
        desconto.Activate();

        await _discountRepository.SaveChangesAsync();
    }

    public async Task Update(Desconto desconto, UpdateCoupomInputModel atualizarCupomInputModel)
    {
        desconto.Update(desconto.Cupom, desconto.PorcDesconto);

        await _discountRepository.SaveChangesAsync();
    }

    public async Task<int> CreateAsync(NewCoupomInputModel novoDescontoInputModel)
    {
        var desconto = new Desconto(novoDescontoInputModel.idEvento,
            novoDescontoInputModel.cupom,
            novoDescontoInputModel.porcentagem,
            novoDescontoInputModel.ativo);

        await _discountRepository.CreateAsync(desconto);
        await _discountRepository.SaveChangesAsync();

        return desconto.Id;
    }

    public async Task<Desconto> GetByIdAsync(int id)
    {
        return await _discountRepository.QueryByIdAsync(id);
    }

    public async Task<List<Desconto>> GetByEventAsync(int idEvento)
    {
        return await _discountRepository.QueryByEventAsync(idEvento);
    }

    public async Task DeleteAsync(Desconto desconto)
    {
        _discountRepository.Delete(desconto);

        await _discountRepository.SaveChangesAsync();
    }

    public async Task DisableAsync(Desconto desconto)
    {
        desconto.Desactivate();

        await _discountRepository.SaveChangesAsync();
    }

    public async Task<Desconto> ValidateAsync(string coupom, int idEvent)
    {
        return await _discountRepository.QueryValidateAsync(coupom, idEvent);
    }
}