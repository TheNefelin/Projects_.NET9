namespace ProjectPortfolio.Domain.Interfaces;

public interface IRepositoryPortfolioBase<T>
{
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
}
