using Dapper;
using Infrastructure.Data;
using ProjectPortfolio.Domain.Entities;
using ProjectPortfolio.Domain.Interfaces;

namespace ProjectPortfolio.Infrastructure.Repositories;

public class ProjectRepository : IRepositoryPortfolioBase<Project>
{
    private readonly IDapperContext _dapper;

    public ProjectRepository(IDapperContext dapper)
    {
        _dapper = dapper;
    }

    public async Task<IEnumerable<Project>> GetAllAsync(CancellationToken cancellationToken)
    {
        var commandDefinition = new CommandDefinition(
            commandText: "SELECT Id, Name, ImgUrl, RepoUrl, AppUrl FROM PF_Projects ORDER BY Id DESC",
            cancellationToken: cancellationToken
        );

        using var connection = _dapper.CreateConnection();
        return await connection.QueryAsync<Project>(commandDefinition);
    }

    public Task<int> CreateAsync(Project entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
