using Infrastructure.Models;
using ProjectPortfolio.Application.Interfaces;
using ProjectPortfolio.Domain.Entities;
using ProjectPortfolio.Domain.Interfaces;

namespace ProjectPortfolio.Application.Services;

public class TechnologyService : IServicePortfolioBase<Technology>
{
    private readonly IRepositoryPortfolioBase<Technology> _repository;

    public TechnologyService(IRepositoryPortfolioBase<Technology> repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<IEnumerable<Technology>>> GetAllAsync(CancellationToken cancellationToken)
    {
        try
        {
            var data = await _repository.GetAllAsync(cancellationToken);
            return new ApiResponse<IEnumerable<Technology>>
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Ok",
                Data = data
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<Technology>>
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
            };
        }
    }

    public Task<ApiResponse<Technology?>> CreateAsync(Technology entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<Technology?>> UpdateAsync(Technology entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<object>> DeleteAsync(int Id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
