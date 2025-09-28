using Dapper;
using Infrastructure.Data;
using ProjectPortfolio.Domain.Entities;
using ProjectPortfolio.Domain.Interfaces;

namespace ProjectPortfolio.Infrastructure.Repositories;

public class LanguageRepository : IRepositoryPortfolioBase<Language>
{
    private readonly IDapperContext _dapper;

    public LanguageRepository(IDapperContext dapper)
    {
        _dapper = dapper;
    }

    public async Task<IEnumerable<Language>> GetAllAsync(CancellationToken cancellationToken)
    {
        var commandDefinition = new CommandDefinition(
            commandText: "SELECT Id, Name, ImgUrl FROM PF_Languages",
            cancellationToken: cancellationToken
        );

        using var connection = _dapper.CreateConnection();
        return await connection.QueryAsync<Language>(commandDefinition);
    }

    public Task<int> CreateAsync(Language entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<int> UpdateAsync(Language entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<int> DeleteAsync(int Id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
