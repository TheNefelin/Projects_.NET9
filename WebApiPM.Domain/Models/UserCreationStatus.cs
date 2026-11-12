namespace WebApiPM.Domain.Models;

/// <summary>
/// Resultado del alta de usuario. Sustituye al contrato de respuesta del
/// stored procedure <c>Auth_Register</c>: el INSERT es parametrizado y se
/// resuelve en C#, asi que el repositorio solo informa el resultado.
/// </summary>
public enum UserCreationStatus
{
    Created,
    EmailAlreadyExists
}
