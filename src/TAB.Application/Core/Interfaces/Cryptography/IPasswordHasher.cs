namespace TAB.Application.Core.Interfaces.Cryptography;

public interface IPasswordHasher
{
    string HashPassword(string password);
    bool VerifyPassword(string hashedPassword, string providedPassword);
}
