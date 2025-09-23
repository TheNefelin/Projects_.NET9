using Dapper;
using Infrastructure.Data;
using ProjectGamesGuide.Domain.Entities;
using ProjectGamesGuide.Domain.Interfaces;

namespace ProjectGamesGuide.Infrastructure.Repositories;

public class SourceRepository : IRepositoryBase<Source>
{
    private readonly IDapperContext _dapper;

    public SourceRepository(IDapperContext dapper)
    {
        _dapper = dapper;
    }

    public async Task<IEnumerable<Source>> GetAllAsync(CancellationToken cancellationToken)
    {
        var commandDefinition = new CommandDefinition(
            commandText: "SELECT Source_Id, Name, Url, Game_Id FROM GG_Sources",
            cancellationToken: cancellationToken
        );

        using var connection = _dapper.CreateConnection();
        return await connection.QueryAsync<Source>(commandDefinition);
    }
}
