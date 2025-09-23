namespace ProjectAuth.Application.Interfaces;

public interface IMaeConfigService
{
    Task<bool> ValidateApiKey(string apiKey);
}
