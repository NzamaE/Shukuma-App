using System.Security.Cryptography;
using System.Text;
using shukuma.domain.Interfaces;

namespace shukuma.persistence.firebase;

public class HashService : IHashService
{
    public string GetHash(string text)
    {
        using var md = MD5.Create();
        var bytes = Encoding.UTF8.GetBytes(text);
        var hash = md.ComputeHash(bytes);
        return Convert.ToBase64String(hash);
    }
}
