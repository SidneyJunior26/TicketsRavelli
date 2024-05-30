namespace TicketsRavelli.Controllers.Inscricao;

public record SubscriptionAthleteViewModel(int Id, int IdEvento, string NomeEvento, string NomeCategoria, bool? Pago,
    bool InscricoesAbertas, string LinkBoleto, bool PermiteNovoBoleto, int? Pacote);
