namespace WebApiPM.Application.Common;

public sealed class RegistrationDisabledException : Exception
{
    public RegistrationDisabledException() : base("El registro de usuarios est\u00e1 deshabilitado.")
    {
    }
}