using WebApiPM.Domain.Entities;

namespace WebApiPM.Domain.Interfaces;

public interface ICoreDataRepository
{
    Task<IEnumerable<CoreData>> GetAllAsync(CoreData coreData, CancellationToken cancellationToken);
    Task<CoreData> InsertAsync(CoreData coreData, CancellationToken cancellationToken);

    /// <summary>
    /// Devuelve false si el <c>Data_Id</c> no existe para ese usuario (la capa
    /// de aplicacion lo traduce a 404). Ignorarlo devolveria un exito falso.
    /// </summary>
    Task<bool> UpdateAsync(CoreData coreData, CancellationToken cancellationToken);

    /// <inheritdoc cref="UpdateAsync"/>
    Task<bool> DeleteAsync(CoreData coreData, CancellationToken cancellationToken);
}