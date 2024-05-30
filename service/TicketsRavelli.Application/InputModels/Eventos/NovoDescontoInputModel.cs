namespace TicketsRavelli.Application.InputModels.Eventos;

public record NewCoupomInputModel(int idEvento, string cupom, int porcentagem, int ativo);