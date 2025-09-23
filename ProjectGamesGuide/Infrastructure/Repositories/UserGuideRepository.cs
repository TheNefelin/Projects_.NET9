using Dapper;
using Infrastructure.Data;
using Infrastructure.Models;
using ProjectGamesGuide.Domain.Entities;
using ProjectGamesGuide.Domain.Interfaces;
using System.Data;

namespace ProjectGamesGuide.Infrastructure.Repositories;

public class UserGuideRepository : IRepositoryByUser<UserGuide>
{
    private readonly IDapperContext _dapper;

    public UserGuideRepository(IDapperContext dapper)
    {
        _dapper = dapper;
    }

    public async Task<IEnumerable<UserGuide>> GetAllByUserIdAsync(Guid User_Id, CancellationToken cancellationToken)
    {
        var commandDefinition = new CommandDefinition(
            commandText: "SELECT User_Id, Guide_Id, IsCheck FROM GG_UserGuides WHERE User_Id = @User_Id",
            parameters: new { User_Id },
            cancellationToken: cancellationToken
        );

        using var connection = _dapper.CreateConnection();
        return await connection.QueryAsync<UserGuide>(commandDefinition);
    }

    public async Task<SqlResponse> UpdateAsync(UserGuide userData, CancellationToken cancellationToken)
    {
        var commandDefinition = new CommandDefinition(
            commandType: CommandType.StoredProcedure,
            commandText: "GG_UserGuides_Set",
            parameters: new { userData.User_Id, userData.Guide_Id, userData.IsCheck },
            cancellationToken: cancellationToken
        );

        using var connection = _dapper.CreateConnection();
        return await connection.QueryFirstAsync<SqlResponse>(commandDefinition);
    }
}
