namespace TicketsRavelli.Controllers.Regulamentos;

public record NewRegulationInputModel(int idEvento, string regulamento1, string compromisso);

public record UpdateRegulationInputModel(string regulamento1, string compromisso);