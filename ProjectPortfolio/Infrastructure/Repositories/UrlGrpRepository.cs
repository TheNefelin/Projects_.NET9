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

    public Task<int> CreateAsync(UrlGrp entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
