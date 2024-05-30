using TicketsRavelli.Core.Entities.Athletes;
using TicketsRavelli.Core.Entities.Eventos;

namespace TicketsRavelli.Infrastructure.EmailServices.Interfaces
{
    public interface IEmailService
    {
        public void TesteEmail();
        public void EnviarConfirmacaoPagamento(Subscription inscricao);
        public void EnviarCodigoResetSenha(string codigo, Athlete atleta);
        public void EnviarCupomCortesiaEvento(string cupom, Evento evento, string nomeAtleta, string emailAtleta);
        public void EnviarNovoBoleto(Subscription inscricao);
    }
}

