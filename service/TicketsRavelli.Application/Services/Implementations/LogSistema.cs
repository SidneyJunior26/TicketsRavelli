

using TicketsRavelli.Application.Services.Interfaces;

namespace TicketsRavelli.Application.Services.Implementations;

public class LogSistema : ILogSystem {
    private readonly string logDirectoryPath = @"C:\inetpub\wwwroot\publish\logs";

    public void SaveLog(string message) {
        // Verifica se o diretório existe, se não, cria-o
        if (!Directory.Exists(logDirectoryPath)) {
            Directory.CreateDirectory(logDirectoryPath);
        }

        // Gera um nome de arquivo com base na data e hora atual
        string timestamp = DateTime.Now.ToString("ddMMyyyyHHmm");
        var logFileName = $"log_{timestamp}.txt";

        try {
            // Caminho completo do arquivo de log
            string logFilePath = Path.Combine(logDirectoryPath, logFileName);

            // Gera a mensagem de log com timestamp
            string logMessage = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";

            // Escreve a mensagem de log no arquivo
            File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
        } catch (Exception ex) {
            // Em caso de erro, pode tratar de acordo com sua necessidade
            Console.WriteLine($"Erro ao salvar log: {ex.Message}");
        }
    }
}

