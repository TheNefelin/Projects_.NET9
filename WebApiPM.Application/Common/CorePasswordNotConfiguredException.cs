namespace WebApiPM.Application.Common;

public sealed class CorePasswordNotConfiguredException : Exception
{
    public CorePasswordNotConfiguredException() : base("Debes crear una clave de encriptaci\u00f3n.")
    {
    }
}