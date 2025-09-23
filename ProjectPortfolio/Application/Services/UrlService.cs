using Infrastructure.Models;
using ProjectPortfolio.Application.Interfaces;
using ProjectPortfolio.Domain.Entities;
using ProjectPortfolio.Domain.Interfaces;

namespace ProjectPortfolio.Application.Services;

public class UrlService : IServicePortfolioBase<Url>
{
    private readonly IRepositoryPortfolioBase<Url> _repository;

    public UrlService(IRepositoryPortfolioBase<Url> repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<IEnumerable<Url>>> GetAllAsync(CancellationToken cancellationToken)
    {
        try
        {
            var data = await _repository.GetAllAsync(cancellationToken);
            return new ApiResponse<IEnumerable<Url>>
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Ok",
                Data = data
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<Url>>
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
            };
        }
    }
}
