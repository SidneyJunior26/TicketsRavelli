using Newtonsoft.Json.Linq;
using TicketsRavelli.Core.Entities.Eventos;
using TicketsRavelli.Infrastructure.InputModels;

namespace TicketsRavelli.Infrastructure.GerenciaNet.Interfaces
{
    public interface IBoletoService
    {
        JObject Generate(string idTransacao, TransacaoInputModel boletoInputModel, Evento evento);
        JObject GetNotificationGn(string token);
        JObject ConsultarStatusBoletos(int chargeId);
    }
}

