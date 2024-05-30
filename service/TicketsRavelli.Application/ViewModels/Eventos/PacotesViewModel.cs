using TicketsRavelli.Core.Entities.Eventos;

namespace TicketsRavelli.Controllers.Eventos;

public class PacotesViewModel
{
    public bool Pacote1Ativo { get; set; }
    public string DescricaoPacote1 { get; set; }
    public decimal? ValorPacote1 { get; set; }
    public bool Pacote2Ativo { get; set; }
    public string DescricaoPacote2 { get; set; }
    public decimal ValorPacote2 { get; set; }
    public bool Pacote3Ativo { get; set; }
    public string DescricaoPacote3 { get; set; }
    public decimal ValorPacote3 { get; set; }
    public bool Pacote4Ativo { get; set; }
    public string DescricaoPacote4 { get; set; }
    public decimal ValorPacote4 { get; set; }

    public PacotesViewModel(Evento evento)
    {
        Pacote1Ativo = Convert.ToBoolean(evento.Pacote1Ativo);
        Pacote2Ativo = Convert.ToBoolean(evento.Pacote2Ativo);
        Pacote3Ativo = Convert.ToBoolean(evento.Pacote3Ativo);
        Pacote4Ativo = Convert.ToBoolean(evento.Pacote4Ativo);

        DescricaoPacote1 = evento.Pacote1Desc;
        DescricaoPacote2 = evento.Pacote2Desc;
        DescricaoPacote3 = evento.Pacote3Desc;
        DescricaoPacote4 = evento.Pacote4Desc;

        if (DateTime.Today < evento.DataDesconto) {
            ValorPacote1 = evento.Valor1;
            ValorPacote2 = evento.Pacote2V1;
            ValorPacote3 = evento.Pacote3V1;
            ValorPacote4 = evento.Pacote4V1;
        } else if (DateTime.Today >= evento.DataDesconto && DateTime.Today < evento.DataValorNormal) {
            ValorPacote1 = evento.Valor2;
            ValorPacote2 = evento.Pacote2V2;
            ValorPacote3 = evento.Pacote3V2;
            ValorPacote4 = evento.Pacote4V2;
        } else if (DateTime.Today >= evento.DataValorNormal) {
            ValorPacote1 = evento.ValorNormal;
            ValorPacote2 = evento.Pacote2V3;
            ValorPacote3 = evento.Pacote3V3;
            ValorPacote4 = evento.Pacote4V3;
        }
    }
}

