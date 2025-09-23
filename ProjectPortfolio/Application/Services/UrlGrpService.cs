using Infrastructure.Models;
using ProjectPortfolio.Application.Interfaces;
using ProjectPortfolio.Domain.Entities;
using ProjectPortfolio.Domain.Interfaces;

namespace ProjectPortfolio.Application.Services;

public class UrlGrpService : IServicePortfolioBase<UrlGrp>
{
    private readonly IRepositoryPortfolioBase<UrlGrp> _repository;

    public UrlGrpService(IRepositoryPortfolioBase<UrlGrp> repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<IEnumerable<UrlGrp>>> GetAllAsync(CancellationToken cancellationToken)
    {
        try
        {
            var data = await _repository.GetAllAsync(cancellationToken);
            return new ApiResponse<IEnumerable<UrlGrp>>
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Ok",
                Data = data
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<UrlGrp>>
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
            };
        }
    }
}
