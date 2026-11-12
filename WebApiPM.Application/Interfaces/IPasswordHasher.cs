namespace WebApiPM.Application.Interfaces;

public interface IPasswordHasher
{
    (string Hash, string Salt) HashPassword(string password);

    /// <summary>
    /// Re-deriva el hash con una sal existente. Lo usa el cambio de clave
    /// maestra, que conserva la sal (y por lo tanto el IV) del servidor.
    /// </summary>
    string HashPassword(string password, string salt);

    bool VerifyPassword(string password, string hashedPassword, string salt);
}
