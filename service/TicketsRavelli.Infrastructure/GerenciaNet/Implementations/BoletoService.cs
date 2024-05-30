using System.Drawing;
using System.Text;
using Gerencianet.NETCore.SDK;
using Newtonsoft.Json.Linq;
using TicketsRavelli.Core.Entities.Eventos;
using TicketsRavelli.Infrastructure.GerenciaNet.Interfaces;
using TicketsRavelli.Infrastructure.InputModels;

namespace TicketsRavelli.Infrastructure.GerenciaNet.Implementations {
    public class BoletoService : IBoletoService {
        public BoletoService() {
        }

        public JObject GetNotificationGn(string token) {
            dynamic endpoints = new Endpoints(JObject.Parse(File.ReadAllText("credentials.json")));

            var param = new {
                token = token
            };

            try {
                var response = endpoints.GetNotification(param);
                JObject jsonResponse = JObject.Parse(response);
                return jsonResponse;
            } catch (GnException e) {
                return null;
            }
        }

        public JObject Generate(string idTransacao, TransacaoInputModel boletoInputModel, Evento evento) {
            dynamic endpoints = new Endpoints(JObject.Parse(File.ReadAllText("credentials.json")));

            var param = new {
                id = idTransacao
            };

            var body = new {
                payment = new {
                    banking_billet = new {
                        customer = new {
                            name = CorrigirCaracteresEspeciais(boletoInputModel.NomeCliente),
                            cpf = boletoInputModel.Cpf,
                            email = boletoInputModel.Email,
                            phone_number = RemoverMascaraTelefone(boletoInputModel.Telefone),
                        },
                        expire_at = DateTime.Now.AddDays(3).ToString("yyyy-MM-dd"),
                        message = $""
                    }
                }
            };

            try {
                var response = endpoints.PayCharge(param, body);
                JObject jsonResponse = JObject.Parse(response);
                return jsonResponse;
            } catch (GnException e) {
                return null;
            }
        }

        public JObject ConsultarStatusBoletos(int chargeId) {
            dynamic endpoints = new Endpoints(JObject.Parse(File.ReadAllText("credentials.json")));

            var param = new {
                id = chargeId
            };

            try {
                var response = endpoints.DetailCharge(param);
                return JObject.Parse(response);
            } catch (GnException e) {
                return null;
            }
        }

        private string RemoverMascaraTelefone(string telefone) {
            return telefone.Replace("(", "")
                .Replace(")", "")
                .Replace(" ", "")
                .Replace("-", "");
        }

        private string CorrigirCaracteresEspeciais(string valor) {
            // Substitui o caractere "Ã§" por "ç"
            string valorCorrigido = valor.Replace("Ã§", "ç");
            valorCorrigido = valor.Replace("?", "ç");

            return valorCorrigido;
        }
    }
}

