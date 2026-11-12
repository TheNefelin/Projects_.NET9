using System.Security.Cryptography;
using System.Text;
using WebApiPM.Application.Interfaces;
using WebApiPM.Domain.Interfaces;

namespace WebApiPM.Application.Services;

public class MaeConfigService : IMaeConfigService
{
    private readonly IMaeConfigRepository _maeConfigRepository;

    public MaeConfigService(IMaeConfigRepository maeConfigRepository)
    {
        _maeConfigRepository = maeConfigRepository;
    }

    public async Task<bool> ValidateApiKey(string apiKey, CancellationToken cancellationToken)
    {
        // Sin cache a proposito: rotar la ApiKey en Mae_Config surte efecto en
        // el siguiente request. Con una cache de 30 s, una ApiKey filtrada
        // seguia siendo valida durante la ventana de expiracion, y el costo de
        // esta consulta (una fila por PK) es despreciable.
        var sqlApiKey = await _maeConfigRepository.GetApiKeyAsync(cancellationToken);
        if (string.IsNullOrEmpty(apiKey) || string.IsNullOrEmpty(sqlApiKey))
            return false;

        return CryptographicOperations.FixedTimeEquals(
            Encoding.UTF8.GetBytes(apiKey),
            Encoding.UTF8.GetBytes(sqlApiKey));
    }
}
