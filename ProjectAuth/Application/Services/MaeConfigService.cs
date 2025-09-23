using ProjectAuth.Application.Interfaces;
using ProjectAuth.Domain.Interfaces;

namespace ProjectAuth.Application.Services;

public class MaeConfigService : IMaeConfigService
{
    private readonly IMaeConfigRepository _maeConfigRepository;

    public MaeConfigService(IMaeConfigRepository maeConfigRepository)
    {
        _maeConfigRepository = maeConfigRepository;
    }

    public async Task<bool> ValidateApiKey(string apiKey)
    {
        try
        {
            var sqlApiKey = await _maeConfigRepository.GetApiKeyAsync();

            if (string.IsNullOrEmpty(sqlApiKey)) return false;

            return apiKey.Equals(sqlApiKey);
        }
        catch (Exception ex)
        {
            throw new Exception("Error validating API Key.", ex);
        }
    }
}
