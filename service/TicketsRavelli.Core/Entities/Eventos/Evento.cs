using System.ComponentModel.DataAnnotations;
using Flunt.Notifications;
using Flunt.Validations;

namespace TicketsRavelli.Core.Entities.Eventos;

public partial class Evento : Notifiable<Notification> {
    [Key]
    public int Id { get; set; }
    public string Nome { get; set; } = null!;
    public string? Descricao { get; set; }
    public string? Local { get; set; }
    public DateTime Data { get; set; }
    public DateTime DataIniInscricao { get; set; }
    public DateTime DataFimInscricao { get; set; }
    public DateTime DataDesconto { get; set; }
    public DateTime? DataValorNormal { get; set; }
    public decimal Valor1 { get; set; }
    public decimal Valor2 { get; set; }
    public decimal? ValorNormal { get; set; }
    public decimal Pacote2V1 { get; set; }
    public decimal Pacote2V2 { get; set; }
    public decimal Pacote2V3 { get; set; }
    public decimal Pacote3V1 { get; set; }
    public decimal Pacote3V2 { get; set; }
    public decimal Pacote3V3 { get; set; }
    public decimal Pacote4V1 { get; set; }
    public decimal Pacote4V2 { get; set; }
    public decimal Pacote4V3 { get; set; }
    public string Pacote1Desc { get; set; } = null!;
    public string Pacote2Desc { get; set; } = null!;
    public string Pacote3Desc { get; set; } = null!;
    public string Pacote4Desc { get; set; } = null!;
    public int? Pacote1Ativo { get; set; }
    public int? Pacote2Ativo { get; set; }
    public int? Pacote3Ativo { get; set; }
    public int? Pacote4Ativo { get; set; }
    public string Categoria { get; set; } = null!;
    public string? BoletoInf1 { get; set; }
    public string? BoletoInf2 { get; set; }
    public string? BoletoInf3 { get; set; }
    public string? BoletoInstrucao1 { get; set; }
    public string? BoletoInstrucao2 { get; set; }
    public string? BoletoInstrucao3 { get; set; }
    public string? ObsTela { get; set; }
    public string? TxtEmailCadastro { get; set; }
    public string? TxtEmailBaixa { get; set; }
    public int AtivaInscricao { get; set; }
    public int AtivaEvento { get; set; }
    public int AtivaAlteracaoInscricao { get; set; }
    public int? EventoTipo { get; set; }
    public string? Pacote1V1Pseg { get; set; }
    public string? Pacote1V2Pseg { get; set; }
    public string? Pacote1V3Pseg { get; set; }
    public string? Pacote2V1Pseg { get; set; }
    public string? Pacote2V2Pseg { get; set; }
    public string? Pacote2V3Pseg { get; set; }
    public string? Pacote3V1Pseg { get; set; }
    public string? Pacote3V2Pseg { get; set; }
    public string? Pacote3V3Pseg { get; set; }
    public string? Pacote4V1Pseg { get; set; }
    public string? Pacote4V2Pseg { get; set; }
    public string? Pacote4V3Pseg { get; set; }
    public string? Imagem { get; set; }
    public Evento() { }

    public Evento(string nome, string? descricao, string? local, DateTime data,
        DateTime dataIniInscricao, DateTime dataFimInscricao, DateTime dataDesconto,
        DateTime? dataValorNormal, decimal valor1, decimal valor2, decimal valorNormal,
        decimal pacote2V1, decimal pacote2V2, decimal pacote2V3, decimal pacote3V1,
        decimal pacote3V2, decimal pacote3V3, decimal pacote4V1, decimal pacote4V2,
        decimal pacote4V3, string pacote1Desc, string pacote2Desc, string pacote3Desc,
        string pacote4Desc, int? pacote1Ativo, int? pacote2Ativo, int? pacote3Ativo,
        int? pacote4Ativo, string categoria, string? boletoInf1, string? boletoInf2,
        string? boletoInf3, string? boletoInstrucao1, string? boletoInstrucao2,
        string? boletoInstrucao3, string? obsTela, string? txtEmailCadastro,
        string? txtEmailBaixa, int ativaInscricao, int ativaEvento, int ativaAlteracaoInscricao, int? eventoTipo,
        string? pacote1V1Pseg, string? pacote1V2Pseg, string? pacote1V3Pseg,
        string? pacote2V1Pseg, string? pacote2V2Pseg, string? pacote2V3Pseg,
        string? pacote3V1Pseg, string? pacote3V2Pseg, string? pacote3V3Pseg,
        string? pacote4V1Pseg, string? pacote4V2Pseg, string? pacote4V3Pseg) {
        Nome = nome;
        Descricao = descricao;
        Local = local;
        Data = data;
        DataIniInscricao = dataIniInscricao;
        DataFimInscricao = dataFimInscricao;
        DataDesconto = dataDesconto;
        DataValorNormal = dataValorNormal;
        Valor1 = valor1;
        Valor2 = valor2;
        ValorNormal = valorNormal;
        Pacote2V1 = pacote2V1;
        Pacote2V2 = pacote2V2;
        Pacote2V3 = pacote2V3;
        Pacote3V1 = pacote3V1;
        Pacote3V2 = pacote3V2;
        Pacote3V3 = pacote3V3;
        Pacote4V1 = pacote4V1;
        Pacote4V2 = pacote4V2;
        Pacote4V3 = pacote4V3;
        Pacote1Desc = pacote1Desc;
        Pacote2Desc = pacote2Desc;
        Pacote3Desc = pacote3Desc;
        Pacote4Desc = pacote4Desc;
        Pacote1Ativo = pacote1Ativo;
        Pacote2Ativo = pacote2Ativo;
        Pacote3Ativo = pacote3Ativo;
        Pacote4Ativo = pacote4Ativo;
        Categoria = categoria;
        BoletoInf1 = boletoInf1;
        BoletoInf2 = boletoInf2;
        BoletoInf3 = boletoInf3;
        BoletoInstrucao1 = boletoInstrucao1;
        BoletoInstrucao2 = boletoInstrucao2;
        BoletoInstrucao3 = boletoInstrucao3;
        ObsTela = obsTela;
        TxtEmailCadastro = txtEmailCadastro;
        TxtEmailBaixa = txtEmailBaixa;
        AtivaInscricao = ativaInscricao;
        AtivaEvento = ativaEvento;
        AtivaAlteracaoInscricao = ativaAlteracaoInscricao;
        EventoTipo = eventoTipo;
        Pacote1V1Pseg = pacote1V1Pseg;
        Pacote1V2Pseg = pacote1V2Pseg;
        Pacote1V3Pseg = pacote1V3Pseg;
        Pacote2V1Pseg = pacote2V1Pseg;
        Pacote2V2Pseg = pacote2V2Pseg;
        Pacote2V3Pseg = pacote2V3Pseg;
        Pacote3V1Pseg = pacote3V1Pseg;
        Pacote3V2Pseg = pacote3V2Pseg;
        Pacote3V3Pseg = pacote3V3Pseg;
        Pacote4V1Pseg = pacote4V1Pseg;
        Pacote4V2Pseg = pacote4V2Pseg;
        Pacote4V3Pseg = pacote4V3Pseg;
    }

    public void Update(string nome, string? descricao, string? local, DateTime data,
        DateTime dataIniInscricao, DateTime dataFimInscricao, DateTime dataDesconto,
        DateTime? dataValorNormal, decimal valor1, decimal valor2, decimal valorNormal,
        decimal pacote2V1, decimal pacote2V2, decimal pacote2V3, decimal pacote3V1,
        decimal pacote3V2, decimal pacote3V3, decimal pacote4V1, decimal pacote4V2,
        decimal pacote4V3, string pacote1Desc, string pacote2Desc, string pacote3Desc,
        string pacote4Desc, int? pacote1Ativo, int? pacote2Ativo, int? pacote3Ativo,
        int? pacote4Ativo, string categoria, string? boletoInf1, string? boletoInf2,
        string? boletoInf3, string? boletoInstrucao1, string? boletoInstrucao2,
        string? boletoInstrucao3, string? obsTela, string? txtEmailCadastro,
        string? txtEmailBaixa, int ativaInscricao, int ativaEvento, int ativaAlteracaoInscricao,int? eventoTipo,
        string? pacote1V1Pseg, string? pacote1V2Pseg, string? pacote1V3Pseg,
        string? pacote2V1Pseg, string? pacote2V2Pseg, string? pacote2V3Pseg,
        string? pacote3V1Pseg, string? pacote3V2Pseg, string? pacote3V3Pseg,
        string? pacote4V1Pseg, string? pacote4V2Pseg, string? pacote4V3Pseg) {
        Nome = nome;
        Descricao = descricao;
        Local = local;
        Data = data;
        DataIniInscricao = dataIniInscricao;
        DataFimInscricao = dataFimInscricao;
        DataDesconto = dataDesconto;
        DataValorNormal = dataValorNormal;
        Valor1 = valor1;
        Valor2 = valor2;
        ValorNormal = valorNormal;
        Pacote2V1 = pacote2V1;
        Pacote2V2 = pacote2V2;
        Pacote2V3 = pacote2V3;
        Pacote3V1 = pacote3V1;
        Pacote3V2 = pacote3V2;
        Pacote3V3 = pacote3V3;
        Pacote4V1 = pacote4V1;
        Pacote4V2 = pacote4V2;
        Pacote4V3 = pacote4V3;
        Pacote1Desc = pacote1Desc;
        Pacote2Desc = pacote2Desc;
        Pacote3Desc = pacote3Desc;
        Pacote4Desc = pacote4Desc;
        Pacote1Ativo = pacote1Ativo;
        Pacote2Ativo = pacote2Ativo;
        Pacote3Ativo = pacote3Ativo;
        Pacote4Ativo = pacote4Ativo;
        Categoria = categoria;
        BoletoInf1 = boletoInf1;
        BoletoInf2 = boletoInf2;
        BoletoInf3 = boletoInf3;
        BoletoInstrucao1 = boletoInstrucao1;
        BoletoInstrucao2 = boletoInstrucao2;
        BoletoInstrucao3 = boletoInstrucao3;
        ObsTela = obsTela;
        TxtEmailCadastro = txtEmailCadastro;
        TxtEmailBaixa = txtEmailBaixa;
        AtivaInscricao = ativaInscricao;
        AtivaEvento = ativaEvento;
        AtivaAlteracaoInscricao = ativaAlteracaoInscricao;
        EventoTipo = eventoTipo;
        Pacote1V1Pseg = pacote1V1Pseg;
        Pacote1V2Pseg = pacote1V2Pseg;
        Pacote1V3Pseg = pacote1V3Pseg;
        Pacote2V1Pseg = pacote2V1Pseg;
        Pacote2V2Pseg = pacote2V2Pseg;
        Pacote2V3Pseg = pacote2V3Pseg;
        Pacote3V1Pseg = pacote3V1Pseg;
        Pacote3V2Pseg = pacote3V2Pseg;
        Pacote3V3Pseg = pacote3V3Pseg;
        Pacote4V1Pseg = pacote4V1Pseg;
        Pacote4V2Pseg = pacote4V2Pseg;
        Pacote4V3Pseg = pacote4V3Pseg;
    }

    public void UpdateNameImageEvent(string nome) {
        Imagem = nome;
    }

    private void ValidarEvento() {
        var contract = new Contract<Evento>()
            .IsNotNullOrEmpty(this.Nome, "nome")
            .IsNotNullOrEmpty(this.Descricao, "descricao")
            ;

        AddNotifications(contract);
    }
}
