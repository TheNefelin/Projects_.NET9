using Dapper;
using Infrastructure.Data;
using Infrastructure.Models;
using ProjectGamesGuide.Domain.Entities;
using ProjectGamesGuide.Domain.Interfaces;
using System.Data;

namespace ProjectGamesGuide.Infrastructure.Repositories;

public class UserAdventureRepository : IRepositoryByUser<UserAdventure>
{
    private readonly IDapperContext _dapper;

    public UserAdventureRepository(IDapperContext dapper)
    {
        _dapper = dapper;
    }

    public async Task<IEnumerable<UserAdventure>> GetAllByUserIdAsync(Guid User_Id, CancellationToken cancellationToken)
    {
        var commandDefinition = new CommandDefinition(
            commandText: "SELECT User_Id, Adventure_Id, IsCheck FROM GG_UserAdventures WHERE User_Id = @User_Id",
            parameters: new { User_Id },
            cancellationToken: cancellationToken
        );

        using var connection = _dapper.CreateConnection();
        return await connection.QueryAsync<UserAdventure>(commandDefinition);
    }

    public async Task<SqlResponse> UpdateAsync(UserAdventure userData, CancellationToken cancellationToken)
    {
        var commandDefinition = new CommandDefinition(
            commandType: CommandType.StoredProcedure,
            commandText: "GG_UserAdventures_Set",
            parameters: new { userData.User_Id, userData.Adventure_Id, userData.IsCheck },
            cancellationToken: cancellationToken
        );

        using var connection = _dapper.CreateConnection();
        return await connection.QueryFirstAsync<SqlResponse>(commandDefinition);
    }
}
