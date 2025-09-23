namespace ProjectAuth.Domain.Interfaces;

public interface IMaeConfigRepository
{
    Task<string?> GetApiKeyAsync();
}
