using Dapper;
using Infrastructure.Data;
using Infrastructure.Models;
using ProjectGamesGuide.Domain.Entities;
using ProjectGamesGuide.Domain.Interfaces;
using System.Data;

namespace ProjectGamesGuide.Infrastructure.Repositories;

public class UserGoogleRepository : IUserGoogleRepository
{
    private readonly IDapperContext _dapper;

    public UserGoogleRepository(IDapperContext dapper)
    {
        _dapper = dapper;
    }

    public async Task<SqlGoogleResponse> LoginGoogleAsync(UserLoginGoogle login, CancellationToken cancellationToken)
    {
        var commandDefinition = new CommandDefinition(
            commandType: CommandType.StoredProcedure,
            commandText: "GG_Login",
            parameters: new { login.Email, login.GoogleSUB, login.GoogleJTI },
            transaction: default,
            cancellationToken: cancellationToken
         );

        using var connection = _dapper.CreateConnection();
        return await connection.QueryFirstAsync<SqlGoogleResponse>(commandDefinition);
    }

    public async Task<UserLoginGoogle?> ValidateLoginAsync(UserLoggedToken userLoggedToken, CancellationToken cancellationToken)
    {
        var commandDefinition = new CommandDefinition(
            commandText: "SELECT Email, GoogleSUB, GoogleJTI FROM GG_Users WHERE User_Id = @User_Id AND SqlToken = @SqlToken",
            parameters: new { userLoggedToken.User_Id, userLoggedToken.SqlToken },
            cancellationToken: cancellationToken
        );

        using var connection = _dapper.CreateConnection();
        return await connection.QueryFirstOrDefaultAsync<UserLoginGoogle>(commandDefinition);
    }
}
