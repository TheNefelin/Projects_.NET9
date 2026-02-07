using Infrastructure.Models;

namespace ProjectPortfolio.Application.Interfaces;

public interface IServicePortfolioBase<T>
{
    Task<ApiResponse<IEnumerable<T>>> GetAllAsync(CancellationToken cancellationToken);
    Task<ApiResponse<T?>> CreateAsync(T entity, CancellationToken cancellationToken);
    Task<ApiResponse<T?>> UpdateAsync(T entity, CancellationToken cancellationToken);
    Task<ApiResponse<object>> DeleteAsync(int Id, CancellationToken cancellationToken);
}
