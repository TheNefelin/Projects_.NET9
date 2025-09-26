using Dapper;
using Infrastructure.Data;
using ProjectPortfolio.Domain.Entities;
using ProjectPortfolio.Domain.Interfaces;

namespace ProjectPortfolio.Infrastructure.Repositories;

public class Pro_Tech_Repository : IRepositoryPortfolioBase<Pro_Tech>
{
    private readonly IDapperContext _dapper;

    public Pro_Tech_Repository(IDapperContext dapper)
    {
        _dapper = dapper;
    }

    public async Task<IEnumerable<Pro_Tech>> GetAllAsync(CancellationToken cancellationToken)
    {
        var commandDefinition = new CommandDefinition(
            commandText: "SELECT Id_Project, Id_Technology FROM PF_Pro_Tech",
            cancellationToken: cancellationToken
        );

        using var connection = _dapper.CreateConnection();
        return await connection.QueryAsync<Pro_Tech>(commandDefinition);
    }

    public Task<int> CreateAsync(Pro_Tech entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
