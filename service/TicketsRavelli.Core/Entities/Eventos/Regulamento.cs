using System.ComponentModel.DataAnnotations;

namespace TicketsRavelli.Core.Entities.Eventos;

public partial class Regulamento {
    [Key]
    public int IdEvento { get; set; }
    public string Regulamento1 { get; set; } = null!;
    public string Compromisso { get; set; } = null!;
    public Regulamento() { }

    public Regulamento(int idEvento, string regulamento, string compromisso) {
        IdEvento = idEvento;
        Regulamento1 = regulamento;
        Compromisso = compromisso;
    }

    public void Update(string regulamento, string compromisso) {
        Regulamento1 = regulamento;
        Compromisso = compromisso;
    }
}