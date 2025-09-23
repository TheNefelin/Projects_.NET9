using Dapper;
using Infrastructure.Data;
using ProjectGamesGuide.Domain.Entities;
using ProjectGamesGuide.Domain.Interfaces;

namespace ProjectGamesGuide.Infrastructure.Repositories;

public class GameRepository : IRepositoryBase<Game>
{
    private readonly IDapperContext _dapper;

    public GameRepository(IDapperContext dapper)
    {
        _dapper = dapper;
    }

    public async Task<IEnumerable<Game>> GetAllAsync(CancellationToken cancellationToken)
    {
        var commandDefinition = new CommandDefinition(
            commandText: "SELECT Game_Id, Name, Description, ImgUrl, IsEnabled FROM GG_Games",
            cancellationToken: cancellationToken
        );

        using var connection = _dapper.CreateConnection();
        return await connection.QueryAsync<Game>(commandDefinition);
    }
}
