using System.Security.Cryptography;
using System.Text;
using TicketsRavelli.Infrastructure.Security.Interfaces;

namespace TicketsRavelli.Services.Security;

public class CriptografiaService : ICryptographyService {
    public string Criptografar(string input) {
        using (MD5 md5 = MD5.Create()) {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++) {
                sb.Append(hashBytes[i].ToString("x2"));
            }
            return sb.ToString();
        }
    }
}

