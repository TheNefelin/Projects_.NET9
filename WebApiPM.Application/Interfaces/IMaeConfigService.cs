namespace WebApiPM.Application.Interfaces;

public interface IMaeConfigService
{
    Task<bool> ValidateApiKey(string apiKey, CancellationToken cancellationToken);
}