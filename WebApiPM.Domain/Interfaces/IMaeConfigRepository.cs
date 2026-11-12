namespace WebApiPM.Domain.Interfaces;

public interface IMaeConfigRepository
{
    Task<string?> GetApiKeyAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Indica si el registro esta habilitado (Mae_Config.IsEnableRegister).
    /// Antes lo validaba el stored procedure Auth_Register; con el INSERT
    /// parametrizado la comprobacion vive en el servicio.
    /// </summary>
    Task<bool> IsRegistrationEnabledAsync(CancellationToken cancellationToken);
}
