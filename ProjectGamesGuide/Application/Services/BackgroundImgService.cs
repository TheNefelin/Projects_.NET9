using Infrastructure.Models;
using ProjectGamesGuide.Application.Interfaces;
using ProjectGamesGuide.Domain.Entities;
using ProjectGamesGuide.Domain.Interfaces;

namespace ProjectGamesGuide.Application.Services;

public class BackgroundImgService : IServiceBase<BackgroundImg>
{
    private readonly IRepositoryBase<BackgroundImg> _repository;

    public BackgroundImgService(IRepositoryBase<BackgroundImg> repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<IEnumerable<BackgroundImg>>> GetAllAsync(CancellationToken cancellationToken)
    {
        try
        {
            var data = await _repository.GetAllAsync(cancellationToken);
            return new ApiResponse<IEnumerable<BackgroundImg>>
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Ok",
                Data = data
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<BackgroundImg>>
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
            };
        }
    }
}
