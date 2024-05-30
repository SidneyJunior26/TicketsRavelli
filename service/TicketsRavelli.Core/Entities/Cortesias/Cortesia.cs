using System.ComponentModel.DataAnnotations;

namespace TicketsRavelli.Core.Entities.Cortesias;

public partial class Courtesy {
    [Key]
    public int IdEvento { get; set; }
    public string Cupom { get; set; } = null!;
    public int Ativo { get; set; }

    public Courtesy(int idEvento, string cupom)
    {
        IdEvento = idEvento;
        Cupom = cupom;
        Ativo = 1;
    }

    public void Disable() {
        Ativo = 0;
    }

    public void UpdateStatus() {
        Ativo = Ativo == 0 ? 1 : 0;
    }
}
