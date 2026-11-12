namespace WebApiPM.Application.Common;

public sealed class CorePasswordAlreadyExistsException : Exception
{
    public CorePasswordAlreadyExistsException() : base("Ya tienes una clave de encriptaci\u00f3n creada.")
    {
    }
}