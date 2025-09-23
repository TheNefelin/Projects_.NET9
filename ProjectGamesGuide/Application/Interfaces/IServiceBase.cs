using Infrastructure.Models;

namespace ProjectGamesGuide.Application.Interfaces;

public interface IServiceBase<T>
{
    Task<ApiResponse<IEnumerable<T>>> GetAllAsync(CancellationToken cancellationToken);
}
