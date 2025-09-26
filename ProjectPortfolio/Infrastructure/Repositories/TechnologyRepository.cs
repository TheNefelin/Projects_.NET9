using Dapper;
using Infrastructure.Data;
using ProjectPortfolio.Domain.Entities;
using ProjectPortfolio.Domain.Interfaces;

namespace ProjectPortfolio.Infrastructure.Repositories;

public class TechnologyRepository : IRepositoryPortfolioBase<Technology>
{
    private readonly IDapperContext _dapper;

    public TechnologyRepository(IDapperContext dapper)
    {
        _dapper = dapper;
    }

    public async Task<IEnumerable<Technology>> GetAllAsync(CancellationToken cancellationToken)
    {
        var commandDefinition = new CommandDefinition(
            commandText: "SELECT Id, Name, ImgUrl FROM PF_Technologies",
            cancellationToken: cancellationToken
        );

        using var connection = _dapper.CreateConnection();
        return await connection.QueryAsync<Technology>(commandDefinition);
    }

    public Task<int> CreateAsync(Technology entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
