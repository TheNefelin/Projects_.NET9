using WebApiPM.Domain.Entities;

namespace WebApiPM.Domain.Interfaces;

public interface ICoreUserRepository
{
    Task<CoreUser?> GetCoreUserAsync(CoreUser coreUser, CancellationToken cancellationToken);
    Task RegisterCoreUserPasswordAsync(CoreUser coreUser, CancellationToken cancellationToken);

    /// <summary>
    /// Cambia la clave maestra de forma atomica: rota hash, sal y token de
    /// sesion, y reemplaza los registros de datos re-cifrados por el cliente.
    /// Todo en una sola transaccion (o pasa todo, o no pasa nada).
    /// </summary>
    Task ChangeCorePasswordAsync(
        Guid userId,
        string hash,
        string salt,
        Guid newSqlToken,
        IEnumerable<CoreData> replacementRecords,
        CancellationToken cancellationToken);
}
