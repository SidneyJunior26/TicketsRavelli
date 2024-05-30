using TicketsRavelli.Application.Services.Interfaces;

namespace TicketsRavelli.Application.Services.Implementations;

public class ImagemService : IImagemService {
    private readonly string _uploadFolderPath;

    public ImagemService() {
        _uploadFolderPath = Path.Combine("C:/xampp/htdocs/assets/Images/Eventos");
    }

    public async Task SaveImageEventAsync(string nomeArquivo, byte[] image) {
        // Certifica de que o diretório de upload exista
        if (!Directory.Exists(_uploadFolderPath)) {
            Directory.CreateDirectory(_uploadFolderPath);
        }

        string filePath = Path.Combine(_uploadFolderPath, nomeArquivo + ".png");

        if (File.Exists(filePath)) {
            File.Delete(filePath);
        }

        // Salva a imagem no disco
        await File.WriteAllBytesAsync(filePath, image);
    }
}