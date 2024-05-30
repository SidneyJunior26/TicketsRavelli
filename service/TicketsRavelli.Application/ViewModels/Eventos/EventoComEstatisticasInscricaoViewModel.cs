using TicketsRavelli.Core.Entities.Eventos;

namespace TicketsRavelli.Application.ViewModels.Eventos;

public class EventoComEstatisticasInscricao {
    public Evento Evento { get; set; }
    public int QtdeInscricoesPagas { get; set; }
    public int QtdInscricoes { get; set; }
}