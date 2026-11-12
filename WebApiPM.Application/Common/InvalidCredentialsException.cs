namespace WebApiPM.Application.Common;

public sealed class InvalidCredentialsException : Exception
{
    public InvalidCredentialsException() : base("Usuario o contrase\u00f1a incorrecta.")
    {
    }
}