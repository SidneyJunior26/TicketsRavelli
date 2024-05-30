using System.ComponentModel.DataAnnotations;
using TicketsRavelli.Core.Entities.Athletes;
using TicketsRavelli.Core.Enums;

namespace TicketsRavelli.Core.Entities.Eventos;

public class Subscription {
    [Key]
    public int Id { get; set; }
    public int IdEvento { get; set; }
    public Evento Evento { get; set; }
    public string CpfAtleta { get; set; }
    public Athlete Atleta { get; set; }
    public int IdSubcategoria { get; set; }
    public Subcategoria Subcategoria { get; set; }
    public string? Equipe { get; set; }
    public string? Dupla { get; set; }
    public string? Quarteto { get; set; }
    public string? Numeral { get; set; }
    public DateTime DataInscricao { get; set; }
    public DateTime? DataEfetivacao { get; set; }
    public bool? Pago { get; set; }
    public decimal? ValorPago { get; set; }
    public int? Pacote { get; set; }
    public bool AceiteRegulamento { get; set; }
    public bool? Cancelado { get; set; }
    public DateTime? GnExpireAt { get; set; }
    public string? GnChargeTxId { get; set; }
    public int? GnChargeId { get; set; }
    public int? GnTotal { get; set; }
    public string? GnLink { get; set; }
    public string? GnBarcode { get; set; }
    public string? GnStatus { get; set; }
    public string? AfiliadoId { get; set; }
    public Subscription() { }

    public Subscription(int idEvento, string cpfAtleta, int idSubcategoria,
        string? equipe, string? dupla, string? quarteto, string? numeral,
        int? pacote, bool aceiteRegulamento, string? afiliadoId, bool? pago) {
        IdEvento = idEvento;
        CpfAtleta = cpfAtleta;
        IdSubcategoria = idSubcategoria;
        Equipe = equipe;
        Dupla = dupla;
        Quarteto = quarteto;
        Numeral = numeral;
        DataInscricao = DateTime.Now;
        if (pago != null)
            Pago = pago;
        else
            Pago = false;
        Pacote = pacote;
        AceiteRegulamento = aceiteRegulamento;
        AfiliadoId = afiliadoId;
    }

    public void Update(int idSubcategoria, string equipe, string dupla,
        string quarteto) {
        IdSubcategoria = idSubcategoria;
        Equipe = equipe;
        Dupla = dupla;
        Quarteto = quarteto;
    }

    public void UpdatePaymentPixInfo(int tempoExpiracao, string gnQrCode,
        string gnLink, string gnStatus) {
        GnExpireAt = DateTime.Now.AddSeconds(tempoExpiracao);
        GnBarcode = gnQrCode;
        GnLink = gnLink;
        GnStatus = gnStatus;
    }

    public void ConfirmPayment(decimal valorPago) {
        ValorPago = valorPago;
        Pago = true;
        DataEfetivacao = DateTime.Now;
    }

    public void UpdatePaymentStatus(string status) {
        GnStatus = status;

        if (status == "paid")
            Pago = true;
    }

    public void Submit(decimal valorPago, MetodoPagamento metodoPagamento) {
        Pago = true;
        DataEfetivacao = DateTime.Now;
        ValorPago = valorPago;

        if (metodoPagamento == MetodoPagamento.Boleto) {
            GnStatus = "paid";
            GnChargeId = 0;
        } else if (metodoPagamento == MetodoPagamento.Pix) {
            GnStatus = "CONCLUIDO";
            GnChargeTxId = "Efetivado pelo Adm";
        } else if (metodoPagamento == MetodoPagamento.Cortesia) {
            GnStatus = "CORTESIA";
            GnChargeTxId = "Efetivado pelo Adm";
        }
    }

    public void UpdateBilletData(DateTime gnExpiracao, int gnChargeId,
    int gnTotal, string gnLink, string gnBarCode, string gnStatus) {
        GnExpireAt = gnExpiracao;
        GnChargeId = gnChargeId;
        GnTotal = gnTotal;
        GnLink = gnLink;
        GnBarcode = gnBarCode;
        GnStatus = gnStatus;
    }

    public void UpdateTxId(string txId, string status) {
        GnChargeTxId = txId;
        GnStatus = status;
    }

    public void UpdateCpf(string novoCpf) {
        CpfAtleta = novoCpf;
    }
}