namespace TicketsRavelli.Application.Services.Interfaces;

public interface IUtilidadesService
{
    string OcultarEmail(string email);
    bool SenhaValida(string senha);
}

