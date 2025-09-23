using Infrastructure.Models;
using ProjectGamesGuide.Application.Interfaces;
using ProjectGamesGuide.Domain.Entities;
using ProjectGamesGuide.Domain.Interfaces;

namespace ProjectGamesGuide.Application.Services;

public class AdventureImgService : IServiceBase<AdventureImg>
{
    private readonly IRepositoryBase<AdventureImg> _repository;

    public AdventureImgService(IRepositoryBase<AdventureImg> repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<IEnumerable<AdventureImg>>> GetAllAsync(CancellationToken cancellationToken)
    {
        try
        {
            var data = await _repository.GetAllAsync(cancellationToken);
            return new ApiResponse<IEnumerable<AdventureImg>>
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Ok",
                Data = data
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<AdventureImg>>
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
            };
        }
    }
}
