namespace TicketsRavelli.Application.Services.Interfaces;

public interface IImagemService
{
    Task SaveImageEventAsync(string nomeArquivo, byte[] image);
}

