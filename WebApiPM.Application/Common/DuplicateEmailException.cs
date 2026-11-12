namespace WebApiPM.Application.Common;

public sealed class DuplicateEmailException : Exception
{
    public DuplicateEmailException() : base("El correo ya est\u00e1 registrado.")
    {
    }
}