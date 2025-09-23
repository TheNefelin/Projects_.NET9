using Dapper;
using Infrastructure.Data;
using ProjectGamesGuide.Domain.Entities;
using ProjectGamesGuide.Domain.Interfaces;

namespace ProjectGamesGuide.Infrastructure.Repositories;

public class AdventureImgRepository : IRepositoryBase<AdventureImg>
{
    private readonly IDapperContext _dapper;

    public AdventureImgRepository(IDapperContext dapper)
    {
        _dapper = dapper;
    }

    public async Task<IEnumerable<AdventureImg>> GetAllAsync(CancellationToken cancellationToken)
    {
        var commandDefinition = new CommandDefinition(
            commandText: "SELECT AdventureImg_Id, ImgUrl, Sort, Adventure_Id FROM GG_AdventureImgs",
            cancellationToken: cancellationToken
        );

        using var connection = _dapper.CreateConnection();
        return await connection.QueryAsync<AdventureImg>(commandDefinition);
    }
}
