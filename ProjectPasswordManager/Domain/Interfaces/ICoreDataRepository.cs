using ProjectPasswordManager.Domain.Entities;

namespace ProjectPasswordManager.Domain.Interfaces;

public interface ICoreDataRepository
{
    Task<IEnumerable<CoreData>> GetAllAsync(CoreData coreData, CancellationToken cancellationToken);
    Task<CoreData> InsertAsync(CoreData coreData, CancellationToken cancellationToken);
    Task<CoreData> UpdateAsync(CoreData coreData, CancellationToken cancellationToken);
    Task DeleteAsync(CoreData coreDatas, CancellationToken cancellationToken);
}
