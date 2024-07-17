using System.Security.Cryptography;
using System.Text;
using TAB.Application.Core.Interfaces.Cryptography;

namespace TAB.Infrastructure.Cryptography;

public class PasswordHasher : IPasswordHasher
{
    public string HashPassword(string password)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(password));

        StringBuilder builder = new();
        foreach (var t in bytes)
        {
            builder.Append(t.ToString("x2"));
        }

        return builder.ToString();
    }

    public bool VerifyPassword(string hashedPassword, string providedPassword) =>
        HashPassword(providedPassword) == hashedPassword;
}
