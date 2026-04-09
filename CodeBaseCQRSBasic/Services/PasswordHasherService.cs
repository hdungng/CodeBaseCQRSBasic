using System.Security.Cryptography;
using System.Text;

namespace CodeBaseCQRSBasic.Services;

public class PasswordHasherService : IPasswordHasherService
{
    public string Hash(string value)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(value));
        return Convert.ToHexString(bytes);
    }

    public bool Verify(string value, string hash)
    {
        return Hash(value).Equals(hash, StringComparison.Ordinal);
    }
}
