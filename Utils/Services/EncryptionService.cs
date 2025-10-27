using System.Security.Cryptography;
using System.Text;

namespace Utils.Services;

public class EncryptionService : IEncryptionService
{
    public string Encrypt(string plainText, string password, string iv)
    {
        using var aes = Aes.Create();
        aes.Key = GetAesKey(password);
        aes.IV = Convert.FromBase64String(iv);

        ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream();
        using var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write);
        using (var sw = new StreamWriter(cs))
        {
            sw.Write(plainText);
        }

        byte[] encrypted = ms.ToArray();
        return Convert.ToBase64String(encrypted);
    }

    public string Decrypt(string encryptedText, string password, string iv)
    {
        byte[] cipherBytes = Convert.FromBase64String(encryptedText);

        using var aes = Aes.Create();
        aes.Key = GetAesKey(password);
        aes.IV = Convert.FromBase64String(iv);

        ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
        using var ms = new MemoryStream(cipherBytes);
        using var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read);
        using var sr = new StreamReader(cs);

        return sr.ReadToEnd();
    }

    private byte[] GetAesKey(string pass)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(pass);
        while (keyBytes.Length < 32)
            keyBytes = keyBytes.Concat(keyBytes).ToArray();
        return keyBytes.Take(32).ToArray();
    }
}
