using Infrastructure.Models;
using ProjectGamesGuide.Application.Interfaces;
using ProjectGamesGuide.Domain.Entities;
using ProjectGamesGuide.Domain.Interfaces;

namespace ProjectGamesGuide.Application.Services;

public class GuideService : IServiceBase<Guide>
{
    private readonly IRepositoryBase<Guide> _repository;

    public GuideService(IRepositoryBase<Guide> repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<IEnumerable<Guide>>> GetAllAsync(CancellationToken cancellationToken)
    {
        try
        {
            var data = await _repository.GetAllAsync(cancellationToken);
            return new ApiResponse<IEnumerable<Guide>>
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Ok",
                Data = data
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<Guide>>
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
            };
        }
    }
}
