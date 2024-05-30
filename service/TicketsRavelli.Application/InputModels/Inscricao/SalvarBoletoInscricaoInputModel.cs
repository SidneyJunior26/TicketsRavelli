namespace TicketsRavelli.Application.InputModels.Inscricao;

public record SaveBilletInputModel(DateTime gnExpiracao, int gnChargeId,
    int gnTotal, string gnLink, string gnBarCode, string gnStatus);