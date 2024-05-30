using System.Text.RegularExpressions;
using TicketsRavelli.Application.Services.Interfaces;

namespace TicketsRavelli.Application.Services.Implementations;

public class UtilidadesService : IUtilidadesService
{
    public UtilidadesService()
    {
    }

    public string OcultarEmail(string email)
    {
        int indexOfAt = email.IndexOf('@');
        if (indexOfAt == -1) {
            // O email não contém um "@" válido
            return email;
        }

        string prefixo = email.Substring(0, Math.Min(indexOfAt, 3)); // Mantém até 3 caracteres do prefixo
        string sufixo = email.Substring(indexOfAt); // Mantém o sufixo a partir do "@"
        string emailOculto = prefixo.PadRight(indexOfAt, '*') + sufixo;

        return emailOculto;
    }

    public bool SenhaValida(string senha)
    {
        var regex = new Regex(@"^.*(?=.{8,})(?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[!*@#$%^&+=]).*$");

        return regex.IsMatch(senha);
    }
}

