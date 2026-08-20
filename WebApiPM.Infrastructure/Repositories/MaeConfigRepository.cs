using Dapper;
using WebApiPM.Domain.Interfaces;
using WebApiPM.Infrastructure.Data;

namespace WebApiPM.Infrastructure.Repositories;

public class MaeConfigRepository : IMaeConfigRepository
{
    private readonly IDapperContext _dapper;

    public MaeConfigRepository(IDapperContext dapper)
    {
        _dapper = dapper;
    }

    public async Task<string?> GetApiKeyAsync(CancellationToken cancellationToken)
    {
        var commandDefinition = new CommandDefinition(
            commandText: "SELECT ApiKey FROM Mae_Config WHERE Config_Id = @Config_Id",
            parameters: new { Config_Id = 1 },
            cancellationToken: cancellationToken);

        using var connection = _dapper.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<string>(commandDefinition);
    }
}