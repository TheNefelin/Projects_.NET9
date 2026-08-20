namespace WebApiPM.Domain.Interfaces;

public interface IMaeConfigRepository
{
    Task<string?> GetApiKeyAsync(CancellationToken cancellationToken);
}