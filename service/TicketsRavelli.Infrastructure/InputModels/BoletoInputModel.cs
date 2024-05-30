namespace TicketsRavelli.Infrastructure.InputModels;

public class TransacaoInputModel {
    public string NomeCliente { get; set; }
    public string Email { get; set; }
    public string Cpf { get; set; }
    public string Telefone { get; set; }
    public string NomeProduto { get; set; }
    public string Valor { get; set; }
    public int Quantidade { get; set; }
    public string CupomDesconto { get; set; }

    public void UpdateValue(string novoValor) {
        Valor = novoValor;
    }
};