namespace WebApiPM.Application.Common;

public sealed class TooManyLoginAttemptsException : Exception
{
    public TooManyLoginAttemptsException() : base("Demasiados intentos fallidos de inicio de sesi\u00f3n. Intenta nuevamente m\u00e1s tarde.")
    {
    }
}