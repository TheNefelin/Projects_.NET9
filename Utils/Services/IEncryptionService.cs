namespace Utils.Services;

public interface IEncryptionService
{
    string Encrypt(string plainText, string password, string iv);
    string Decrypt(string encryptedText, string password, string iv);
}
