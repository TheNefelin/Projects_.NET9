namespace WebApiPM.Application.Common;

public sealed class UserSessionInvalidException : Exception
{
    public UserSessionInvalidException() : base("Debes iniciar sesi\u00f3n.")
    {
    }
}