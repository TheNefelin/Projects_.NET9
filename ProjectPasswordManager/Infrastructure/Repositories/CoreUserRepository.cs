using Dapper;
using Infrastructure.Data;
using ProjectPasswordManager.Domain.Entities;
using ProjectPasswordManager.Domain.Interfaces;

namespace ProjectPasswordManager.Infrastructure.Repositories;

public class CoreUserRepository : ICoreUserRepository
{
    private readonly IDapperContext _dapper;

    public CoreUserRepository(IDapperContext dapper)
    {
        _dapper = dapper;
    }

    public async Task<CoreUser?> GetCoreUserAsync(CoreUser coreUser, CancellationToken cancellationToken)
    {
        var commandDefinition = new CommandDefinition(
            cancellationToken: cancellationToken,
            commandText: "SELECT User_Id, HashPM, SaltPM, SqlToken FROM Auth_Users WHERE User_Id = @User_Id AND SqlToken = @SqlToken",
            parameters: new
            {
                coreUser.User_Id,
                coreUser.SqlToken
            }
        );

        using var connection = _dapper.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<CoreUser>(commandDefinition);
    }

    public async Task RegisterCoreUserPasswordAsync(CoreUser coreUser, CancellationToken cancellationToken)
    {
        var commandDefinition = new CommandDefinition(
            cancellationToken: cancellationToken,
            commandText: $"UPDATE Auth_Users SET HashPM = @HashPM, SaltPM = @SaltPM WHERE User_Id = @User_Id AND SqlToken = @SqlToken",
            parameters: new
            {
                coreUser.User_Id,
                coreUser.SqlToken,
                coreUser.HashPM,
                coreUser.SaltPM
            }
        );

        using var connection = _dapper.CreateConnection();
        await connection.QueryAsync(commandDefinition);
    }
}
