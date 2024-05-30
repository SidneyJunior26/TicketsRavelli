namespace TicketsRavelli.Application.InputModels.Pagamentos.Pix;

public record NovoPagamentoPixInputModel(string txId, string qrCode, Int32 tempoExpiracao, string status);