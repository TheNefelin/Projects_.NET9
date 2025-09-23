using Infrastructure.Models;
using ProjectGamesGuide.Application.Interfaces;
using ProjectGamesGuide.Domain.Entities;
using ProjectGamesGuide.Domain.Interfaces;

namespace ProjectGamesGuide.Application.Services;

public class SourceService : IServiceBase<Source>
{
    private readonly IRepositoryBase<Source> _repository;

    public SourceService(IRepositoryBase<Source> repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<IEnumerable<Source>>> GetAllAsync(CancellationToken cancellationToken)
    {
        try
        {
            var data = await _repository.GetAllAsync(cancellationToken);
            return new ApiResponse<IEnumerable<Source>>
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Ok",
                Data = data
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<Source>>
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
            };
        }
    }
}
