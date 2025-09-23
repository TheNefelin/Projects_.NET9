using Infrastructure.Models;

namespace ProjectGamesGuide.Domain.Interfaces;

public interface IRepositoryByUser<T>
{
    Task<IEnumerable<T>> GetAllByUserIdAsync(Guid User_Id, CancellationToken cancellationToken);
    Task<SqlResponse> UpdateAsync(T userData, CancellationToken cancellationToken);
}
