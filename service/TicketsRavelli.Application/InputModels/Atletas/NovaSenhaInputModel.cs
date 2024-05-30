namespace TicketsRavelli.Application.InputModels.Atletas;

public record NovaSenhaInputModel(string cpf, string senhaAtual, string novaSenha);

public record NovaSenhaCodigoInputModel(string id, string codigo, string novaSenha);

public record EsqueceuSenhaInputModel(string cpfEmail, string codigo, string novaSenha);

public record ManagerNovaSenha(string id, string novaSenha);