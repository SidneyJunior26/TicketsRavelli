namespace TicketsRavelli.Controllers.Inscricao;

public record SubscriptionInputModel(int idEvento, string cpfAtleta, int idSubcategoria,
    string equipe, string dupla, string quarteto, string? numeral, DateTime? dataInscricao,
    DateTime? dataEfetivacao, bool? pago, decimal? valorPago, int? pacote, bool aceiteRegulamento,
    bool? cancelado, DateTime? gnExpireAt, int? gnChargeId, int? gnTotal, string? gnLink,
    string? gnBarCode, string? gnStatus, string? afiliadoId);