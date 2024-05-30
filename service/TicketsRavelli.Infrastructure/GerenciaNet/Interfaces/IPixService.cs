using Newtonsoft.Json.Linq;

namespace TicketsRavelli.Infrastructure.GerenciaNet.Interfaces;

public interface IPixService
{
    JObject CreateImmediateBilling(string cpfAtleta, string nomeAtleta, decimal valor, string nomeEvento);
    JObject GenerateQrCode(string id);
    JObject GetStatus(string txId);
}

