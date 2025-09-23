namespace ProjectGamesGuide.Domain.Interfaces;

public interface IRepositoryBase<T>
{
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
}
