using Gerencianet.NETCore.SDK;
using Newtonsoft.Json.Linq;
using TicketsRavelli.Infrastructure.GerenciaNet.Interfaces;

namespace TicketsRavelli.Infrastructure.GerenciaNet.Implementations;

public class PixService : IPixService {
    public PixService() {
    }

    public JObject GetStatus(string txId) {
        dynamic endpoints = new Endpoints(JObject.Parse(File.ReadAllText("credentials.json")));

        var param = new {
            txid = txId
        };

        try {
            var response = endpoints.PixDetailCharge(param);
            return JObject.Parse(response);
        } catch (GnException e) {
            return null;
        }
    }

    public JObject CreateImmediateBilling(string cpfAtleta, string nomeAtleta, decimal valor, string nomeEvento) {
        dynamic endpoints = new Endpoints(JObject.Parse(File.ReadAllText("credentials.json")));

        nomeAtleta = CorrigirCaracteresEspeciais(nomeAtleta);

        var body = new {
            calendario = new {
                expiracao = 3600
            },
            devedor = new {
                cpf = cpfAtleta,
                nome = nomeAtleta
            },
            valor = new {
                original = valor.ToString().Replace(",", ".")
            },
            chave = "159841e2-448e-4f61-a3d3-4d7fa505ddbf",
            solicitacaoPagador = $"Pix gerado para confirmar inscrição no evento - {nomeEvento}"
        };

        try {
            var response = endpoints.PixCreateImmediateCharge(null, body);

            return JObject.Parse(response);
        } catch (GnException e) {
            return null;
        } catch (Exception ex) {
            return null;
        }
    }

    public JObject GenerateQrCode(string id) {
        dynamic endpoints = new Endpoints(JObject.Parse(File.ReadAllText("credentials.json")));

        var param = new {
            id = id
        };

        try {
            var response = endpoints.PixGenerateQRCode(param);
            return JObject.Parse(response);

        } catch (GnException e) {
            return null;
        }
    }

    private string CorrigirCaracteresEspeciais(string valor) {
        // Substitui o caractere "Ã§" por "ç"
        string valorCorrigido = valor.Replace("Ã§", "ç");

        return valorCorrigido;
    }
}
