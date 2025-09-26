using ProjectPortfolio.Domain.Entities;

namespace ProjectPortfolio.Domain.Interfaces;

public interface IRepositoryPortfolioBase<T>
{
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
    Task<int> CreateAsync(T entity, CancellationToken cancellationToken);
    Task<int> UpdateAsync(T entity, CancellationToken cancellationToken);
    Task<int> DeleteAsync(int Id, CancellationToken cancellationToken);
}
