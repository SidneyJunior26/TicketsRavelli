using System;
namespace TicketsRavelli.Application.ViewModels.Relatorios
{
    public class RelatorioTotalCamisasCategoriaInputModel
    {
        public RelatorioTotalCamisasCategoriaInputModel(string camisa, string camisaCiclismo, int totalInscritos) {
            Camisa = camisa;
            CamisaCiclismo = camisaCiclismo;
            TotalInscritos = totalInscritos;
        }

        public string Camisa { get; set; }
        public string CamisaCiclismo { get; set; }
        public int TotalInscritos { get; private set; }
    }
}

