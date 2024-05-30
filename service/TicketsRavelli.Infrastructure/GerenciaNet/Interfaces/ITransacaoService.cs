using Newtonsoft.Json.Linq;
using TicketsRavelli.Infrastructure.InputModels;

namespace TicketsRavelli.Infrastructure.GerenciaNet.Interfaces;

public interface ITransacaoService {
    JObject CreateTransaction(TransacaoInputModel boletoInputModel);
}

