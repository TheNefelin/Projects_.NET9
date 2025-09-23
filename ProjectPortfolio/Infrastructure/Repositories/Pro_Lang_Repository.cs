using Dapper;
using Infrastructure.Data;
using ProjectPortfolio.Domain.Entities;
using ProjectPortfolio.Domain.Interfaces;

namespace ProjectPortfolio.Infrastructure.Repositories;

public class Pro_Lang_Repository : IRepositoryPortfolioBase<Pro_Lang>
{
    private readonly IDapperContext _dapper;

    public Pro_Lang_Repository(IDapperContext dapper)
    {
        _dapper = dapper;
    }

    public async Task<IEnumerable<Pro_Lang>> GetAllAsync(CancellationToken cancellationToken)
    {
        var commandDefinition = new CommandDefinition(
            commandText: "SELECT Id_Project, Id_Language FROM PF_Pro_Lang",
            cancellationToken: cancellationToken
        );

        using var connection = _dapper.CreateConnection();
        return await connection.QueryAsync<Pro_Lang>(commandDefinition);
    }
}
