using Dapper;
using Infrastructure.Data;
using ProjectGamesGuide.Domain.Entities;
using ProjectGamesGuide.Domain.Interfaces;

namespace ProjectGamesGuide.Infrastructure.Repositories;

public class AdventureRepository : IRepositoryBase<Adventure>
{
    private readonly IDapperContext _dapper;

    public AdventureRepository(IDapperContext dapper)
    {
        _dapper = dapper;
    }

    public async Task<IEnumerable<Adventure>> GetAllAsync(CancellationToken cancellationToken)
    {
        var commandDefinition = new CommandDefinition(
            commandText: "SELECT Adventure_Id, Description, IsImportant, Sort, Guide_Id FROM GG_Adventures",
            cancellationToken: cancellationToken
        );

        using var connection = _dapper.CreateConnection();
        return await connection.QueryAsync<Adventure>(commandDefinition);
    }
}
