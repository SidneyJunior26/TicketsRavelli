using Gerencianet.NETCore.SDK;
using Newtonsoft.Json.Linq;
using TicketsRavelli.Infrastructure.GerenciaNet.Interfaces;
using TicketsRavelli.Infrastructure.InputModels;

namespace TicketsRavelli.Infrastructure.GerenciaNet.Implementations;

public class TransacaoService : ITransacaoService {
    public TransacaoService() {
    }

    public JObject CreateTransaction(TransacaoInputModel boletoInputModel) {
        dynamic endpoints = new Endpoints(JObject.Parse(File.ReadAllText("credentials.json")));

        int valorBoleto = CorrigeValorBoleto(boletoInputModel.Valor);

        var body = new {
            items = new[] {
                new {
                    name = boletoInputModel.NomeProduto,
                    value = valorBoleto,
                    amount = boletoInputModel.Quantidade
                }
            },
            metadata = new {
                notification_url = "https://api.ticketsravelli.com.br:3032/v1/Boleto/notificacao"
            }
        };

        try {
            var response = endpoints.CreateCharge(null, body);
            JObject jsonResponse = JObject.Parse(response);
            return jsonResponse;
        } catch (GnException e) {
            return null;
        }
    }

    private int CorrigeValorBoleto(string valor) {
        string valorString = valor;
        // Remove a vírgula da string
        string valorSemVirgula = valorString.Replace(",", "").Replace(".", "");
        // Converte a string para um inteiro
        return int.Parse(valorSemVirgula);
    }
}

