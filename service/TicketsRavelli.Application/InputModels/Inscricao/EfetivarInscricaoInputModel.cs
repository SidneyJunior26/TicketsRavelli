using TicketsRavelli.Core.Enums;

namespace TicketsRavelli.Application.InputModels.Inscricao;

public class EffectSubscriptionInputModel
{
    public string ValorPago { get; set; }
    public MetodoPagamento MetodoPagamento { get; set; }
}