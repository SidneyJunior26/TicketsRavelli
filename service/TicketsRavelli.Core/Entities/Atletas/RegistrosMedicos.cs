using System.ComponentModel.DataAnnotations;
using Flunt.Notifications;

namespace TicketsRavelli.Core.Entities.Atletas;

public partial class RegistroMedico : Notifiable<Notification> {
    [Key]
    public string IdAtleta { get; set; }
    public int? Plano { get; set; }
    public string? PlanoEmpresa { get; set; }
    public string? PlanoTipo { get; set; }
    public int Pressaoalta { get; set; }
    public int Desmaio { get; set; }
    public int Cardiaco { get; set; }
    public int Diabetes { get; set; }
    public int Asma { get; set; }
    public int Alergia { get; set; }
    public string? AlergiaQual { get; set; }
    public int Cirurgia { get; set; }
    public string? CirurgiaQual { get; set; }
    public int Medicacao { get; set; }
    public string? MedicacaoQual { get; set; }
    public string? MedicacaoTempo { get; set; }
    public int Malestar { get; set; }
    public string? MalestarQual { get; set; }
    public int Acompanhamento { get; set; }
    public string? AcompanhamentoQual { get; set; }
    public string? Outros { get; set; }
    public RegistroMedico() { }

    public RegistroMedico(string idAtleta, int plano, string planoEmpresa, string planoTipo,
        int pressaoAlta, int desmaio, int cardiaco, int diabetes, int asma, int alergia,
        string alergiaQual, int cirurgia, string cirurgiaQual, int medicacao,
        string medicacaoQual, string medicacaoTempo, int malEstar, string malEstarQual,
        int acompanhamento, string acompanhamentoQual, string outros) {

        IdAtleta = idAtleta;
        Plano = plano;
        PlanoEmpresa = planoEmpresa;
        PlanoTipo = planoTipo;
        Pressaoalta = pressaoAlta;
        Desmaio = desmaio;
        Cardiaco = cardiaco;
        Diabetes = diabetes;
        Asma = asma;
        Alergia = alergia;
        AlergiaQual = alergiaQual;
        Cirurgia = cirurgia;
        CirurgiaQual = cirurgiaQual;
        Medicacao = medicacao;
        MedicacaoQual = medicacaoQual;
        MedicacaoTempo = medicacaoTempo;
        Malestar = malEstar;
        MalestarQual = malEstarQual;
        Acompanhamento = acompanhamento;
        AcompanhamentoQual = acompanhamentoQual;
        Outros = outros;
    }

    public void EditarRegistrosMedicos(int plano, string planoEmpresa, string planoTipo,
        int pressaoAlta, int desmaio, int cardiaco, int diabetes, int asma, int alergia,
        string alergiaQual, int cirurgia, string cirurgiaQual, int medicacao,
        string medicacaoQual, string medicacaoTempo, int malEstar, string malEstarQual,
        int acompanhamento, string acompanhamentoQual, string outros) {

        Plano = plano;
        PlanoEmpresa = planoEmpresa;
        PlanoTipo = planoTipo;
        Pressaoalta = pressaoAlta;
        Desmaio = desmaio;
        Cardiaco = cardiaco;
        Diabetes = diabetes;
        Asma = asma;
        Alergia = alergia;
        AlergiaQual = alergiaQual;
        Cirurgia = cirurgia;
        CirurgiaQual = cirurgiaQual;
        Medicacao = medicacao;
        MedicacaoQual = medicacaoQual;
        MedicacaoTempo = medicacaoTempo;
        Malestar = malEstar;
        MalestarQual = malEstarQual;
        Acompanhamento = acompanhamento;
        AcompanhamentoQual = acompanhamentoQual;
        Outros = outros;
    }
}
