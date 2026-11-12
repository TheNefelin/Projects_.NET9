namespace WebApiPM.Domain.Entities;

public class AuthUser
{
    public Guid User_Id { get; set; }
    public required string Email { get; set; }
    public required string HashLogin { get; set; }
    public required string SaltLogin { get; set; }
    public string? HashPM { get; set; }
    public string? SaltPM { get; set; }

    // SHA-256 en hexadecimal minusculas del token de sesion. El token crudo
    // nunca se persiste: si alguien lee la base no obtiene un token reutilizable.
    public string? SqlTokenHash { get; set; }

    public string? Role { get; set; }
}
