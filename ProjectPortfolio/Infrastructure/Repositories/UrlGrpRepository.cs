using Dapper;
using Infrastructure.Data;
using ProjectPortfolio.Domain.Entities;
using ProjectPortfolio.Domain.Interfaces;

namespace ProjectPortfolio.Infrastructure.Repositories;

public class UrlGrpRepository : IRepositoryPortfolioBase<UrlGrp>
{
    private readonly IDapperContext _dapper;

    public UrlGrpRepository(IDapperContext dapper)
    {
        _dapper = dapper;
    }

    public async Task<IEnumerable<UrlGrp>> GetAllAsync(CancellationToken cancellationToken)
    {
        var commandDefinition = new CommandDefinition(
            commandText: "SELECT Id, Name, IsEnable FROM PF_UrlGrp",
            cancellationToken: cancellationToken
        );

        using var connection = _dapper.CreateConnection();
        return await connection.QueryAsync<UrlGrp>(commandDefinition);
    }

    public async Task<int> CreateAsync(UrlGrp entity, CancellationToken cancellationToken)
    {
        var commandDefinition = new CommandDefinition(
            commandText: @"
                INSERT INTO PF_UrlGrp (Name, IsEnable)
                OUTPUT INSERTED.Id
                VALUES (@Name, 1)",
            parameters: new { entity.Name },
            cancellationToken: cancellationToken
        );

        using var connection = _dapper.CreateConnection();
        return await connection.ExecuteScalarAsync<int>(commandDefinition);
    }

    public async Task<int> UpdateAsync(UrlGrp entity, CancellationToken cancellationToken)
    {
        var commandDefinition = new CommandDefinition(
            commandText: @"
                UPDATE PF_UrlGrp SET 
                    Name = @Name, 
                    IsEnable = @IsEnable
                WHERE Id = @Id",
            parameters: new { entity.Name, entity.IsEnable, entity.Id },
            cancellationToken: cancellationToken
        );

        using var connection = _dapper.CreateConnection();
        return await connection.ExecuteAsync(commandDefinition);
    }

    public async Task<int> DeleteAsync(int Id, CancellationToken cancellationToken)
    {
        var commandDefinition = new CommandDefinition(
            commandText: "DELETE FROM PF_UrlGrp WHERE Id = @Id",
            parameters: new { Id },
            cancellationToken: cancellationToken
        );

        using var connection = _dapper.CreateConnection();
        return await connection.ExecuteAsync(commandDefinition);
    }
}
