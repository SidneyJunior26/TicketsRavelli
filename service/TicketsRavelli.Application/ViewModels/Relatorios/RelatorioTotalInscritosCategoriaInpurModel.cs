using System;
namespace TicketsRavelli.Application.ViewModels.Relatorios
{
    public class RelatorioTotalInscritosCategoriaInpurModel
    {
        public RelatorioTotalInscritosCategoriaInpurModel(string subCategoria, int totalInscritos)
        {
            SubCategoria = subCategoria;
            TotalInscritos = totalInscritos;
        }

        public string SubCategoria { get; private set; }
        public int TotalInscritos { get; private set; }
    }
}