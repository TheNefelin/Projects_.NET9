using Infrastructure.Models;

namespace ProjectPortfolio.Domain.Interfaces;

public interface IRepositoryPortfolioPaged<T>
{
    Task<PaginatedResult<T>> GetPagedAsync(
        int page,
        int pageSize,
        string? orderBy = null,
        bool ascending = true,
        CancellationToken cancellationToken = default);
}