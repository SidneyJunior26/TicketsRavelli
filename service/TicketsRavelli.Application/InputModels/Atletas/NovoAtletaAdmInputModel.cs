namespace TicketsRavelli.Application.InputModels.Atletas;

public record NewAthleteAdmInputModel(string Nome, string Nascimento, string Sexo,
    string Cpf, string Rg, string Responsavel, string Endereco, string Numero, string Complemento,
    string Cep, string Cidade, string Uf, string Pais, string Telefone,
    string Celular, string Email, string Profissao, string EmergenciaContato,
    string EmergenciaFone, string EmergenciaCelular,
    string Camisa, string CamisaCiclismo, string MktLojaPreferida,
    string MktBikePreferida, string MktAro, string MktCambio,
    string MktFreio, string MktSuspensao, string MktMarcaPneu,
    string MktModeloPneu, string MktTenis,
    string Federacao, string Acesso);