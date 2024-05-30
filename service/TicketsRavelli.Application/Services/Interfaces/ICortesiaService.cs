using TicketsRavelli.Core.Entities.Cortesias;

namespace TicketsRavelli.Application.Services.Interfaces
{
    public interface ICortesiaService
    {
        Task<Cortesia?> ConsultarCupomCortesiaAtivoEventoAsync(int idEvento, string cupom);
        Task<List<Cortesia>> ConsultarCuponsEvento(int IdEvento);
        Task<Cortesia> ConsultarCupomCortesia(string cupom);
        Task AlterarStatusCupom(Cortesia cortesia);
        Task DesativarCupom(Cortesia cortesia);
        Task CadastrarCortesiaEventoAsync(string cupom, int idEvento);
        Task ApagarCupom(Cortesia cortesia);
    }
}

