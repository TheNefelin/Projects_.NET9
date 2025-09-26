using Dapper;
using Infrastructure.Data;
using ProjectPortfolio.Domain.Entities;
using ProjectPortfolio.Domain.Interfaces;

namespace ProjectPortfolio.Infrastructure.Repositories;

public class UrlRepository : IRepositoryPortfolioBase<Url>
{
    private readonly IDapperContext _dapper;

    public UrlRepository(IDapperContext dapper)
    {
        _dapper = dapper;
    }

    public async Task<IEnumerable<Url>> GetAllAsync(CancellationToken cancellationToken)
    {
        var commandDefinition = new CommandDefinition(
            commandText: "SELECT Id, Name, Link, IsEnable, Id_UrlGrp FROM PF_Url",
            cancellationToken: cancellationToken
        );

        using var connection = _dapper.CreateConnection();
        return await connection.QueryAsync<Url>(commandDefinition);
    }

    public async Task<int> CreateAsync(Url entity, CancellationToken cancellationToken)
    {
        var commandDefinition = new CommandDefinition(
            commandText: @"
                INSERT INTO PF_Url (Name, Link, IsEnable, Id_UrlGrp)
                OUTPUT INSERTED.Id
                VALUES (@Name, @Link, 1, @Id_UrlGrp)",
            parameters: new { entity.Name, entity.Link, entity.Id_UrlGrp },
            cancellationToken: cancellationToken
        );

        using var connection = _dapper.CreateConnection();
        return await connection.ExecuteScalarAsync<int>(commandDefinition);
    }

    public async Task<int> UpdateAsync(Url entity, CancellationToken cancellationToken)
    {
        var commandDefinition = new CommandDefinition(
            commandText: @"
                UPDATE PF_Url SET 
                    Name = @Name, 
                    Link = @Link,
                    IsEnable = @IsEnable,
                    Id_UrlGrp = @Id_UrlGrp
                WHERE Id = @Id",
            parameters: new { entity.Name, entity.Link, entity.IsEnable, entity.Id_UrlGrp, entity.Id },
            cancellationToken: cancellationToken
        );

        using var connection = _dapper.CreateConnection();
        return await connection.ExecuteAsync(commandDefinition);
    }

    public async Task<int> DeleteAsync(int Id, CancellationToken cancellationToken)
    {
        var commandDefinition = new CommandDefinition(
            commandText: "DELETE FROM PF_Url WHERE Id = @Id",
            parameters: new { Id },
            cancellationToken: cancellationToken
        );

        using var connection = _dapper.CreateConnection();
        return await connection.ExecuteAsync(commandDefinition);
    }
}
