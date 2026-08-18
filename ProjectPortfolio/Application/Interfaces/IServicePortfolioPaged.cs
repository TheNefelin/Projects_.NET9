using Infrastructure.Models;

namespace ProjectPortfolio.Application.Interfaces;

public interface IServicePortfolioPaged<T>
{
    Task<PaginatedResult<T>> GetPagedAsync(
        int page,
        int pageSize,
        string? orderBy = null,
        bool ascending = true,
        CancellationToken cancellationToken = default);
}