using System.ComponentModel.DataAnnotations;
using Flunt.Notifications;
using Flunt.Validations;

namespace TicketsRavelli.Core.Entities.Eventos;

public partial class Subcategoria : Notifiable<Notification> {
    [Key]
    public int Id { get; set; }
    public int IdEvento { get; set; }
    public int Categoria { get; set; }
    public string DescSubcategoria { get; set; } = null!;
    public int FiltroSexo { get; set; }
    public int FiltroDupla { get; set; }
    public int IdadeDe { get; set; }
    public int IdadeAte { get; set; }
    public string? Aviso { get; set; }
    public bool Ativo { get; set; }
    public Subcategoria() { }

    public Subcategoria(int idEvento, int categoria, string descSubcategoria,
        int filtroSexo, int filtroDupla, int idadeDe, int idadeAte, string? aviso,
        bool ativo) {
        IdEvento = idEvento;
        Categoria = categoria;
        DescSubcategoria = descSubcategoria;
        FiltroSexo = filtroSexo;
        FiltroDupla = filtroDupla;
        IdadeDe = idadeDe;
        IdadeAte = idadeAte;
        Aviso = aviso;
        Ativo = ativo;
    }

    public void EditarCategoria(int categoria, string descSubcategoria,
        int filtroSexo, int filtroDupla, int idadeDe, int idadeAte, string? aviso,
        bool ativo) {

        Categoria = categoria;
        DescSubcategoria = descSubcategoria;
        FiltroSexo = filtroSexo;
        FiltroDupla = filtroDupla;
        IdadeDe = idadeDe;
        IdadeAte = idadeAte;
        Aviso = aviso;
        Ativo = ativo;
    }

    private void ValidarDados() {
        var contract = new Contract<Subcategoria>()
            .IsNotNullOrEmpty(this.DescSubcategoria, "descSubCategoria")
            .IsGreaterThan(this.IdadeDe, 0, "idadeDe")
            .IsGreaterThan(this.IdadeAte, 0, "idadeAte")
            ;

        AddNotifications(contract);
    }
}
