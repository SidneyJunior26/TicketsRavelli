using System.ComponentModel.DataAnnotations;

namespace TicketsRavelli.Core.Entities.Descontos
{
    public class Desconto
    {
        [Key]
        public int Id { get; private set; }
        public int IdEvento { get; private set; }
        public string Cupom { get; private set; }
        public int PorcDesconto { get; private set; }
        public int Ativo { get; private set; }

        public Desconto(int idEvento, string cupom, int porcDesconto, int ativo)
        {
            IdEvento = idEvento;
            Cupom = cupom;
            PorcDesconto = porcDesconto;
            Ativo = ativo;
        }

        public void Update(string cupom, int porcDesconto) {
            Cupom = cupom;
            PorcDesconto = porcDesconto;
        }

        public void Desactivate() {
            Ativo = 0;
        }

        public void Activate() {
            Ativo = 1;
        }
    }
}

