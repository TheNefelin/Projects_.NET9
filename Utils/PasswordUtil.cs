using System.Security.Cryptography;

namespace Utils;

public class PasswordUtil
{
    public (string Hash, string Salt) HashPassword(string password)
    {
        byte[] saltBytes = RandomNumberGenerator.GetBytes(16);
        string salted = Convert.ToBase64String(saltBytes);
        string hashed = NewHash(password, saltBytes);

        return (hashed, salted);
    }

    public bool VerifyPassword(string password, string hashedPassword, string salt)
    {
        byte[] saltBytes = Convert.FromBase64String(salt);
        string hashed = NewHash(password, saltBytes);

        return hashed == hashedPassword;
    }

    private string NewHash(string password, byte[] salt)
    {
        // PBKDF2-HMACSHA256 desde el BCL: el paquete externo
        // Microsoft.AspNetCore.Cryptography.KeyDerivation esta obsoleto y solo
        // envolvia Rfc2898DeriveBytes.Pbkdf2.
        return Convert.ToBase64String(Rfc2898DeriveBytes.Pbkdf2(
            password,
            salt,
            100000,
            HashAlgorithmName.SHA256,
            32)
        );
    }
}
