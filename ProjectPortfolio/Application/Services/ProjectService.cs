using Infrastructure.Models;
using ProjectPortfolio.Application.Interfaces;
using ProjectPortfolio.Domain.Entities;
using ProjectPortfolio.Domain.Interfaces;

namespace ProjectPortfolio.Application.Services;

public class ProjectService : IServicePortfolioBase<Project>
{
    private readonly IRepositoryPortfolioBase<Project> _repository;

    public ProjectService(IRepositoryPortfolioBase<Project> repository)
    {
        _repository = repository;
    }

    public async Task<ApiResponse<IEnumerable<Project>>> GetAllAsync(CancellationToken cancellationToken)
    {
        try
        {
            var data = await _repository.GetAllAsync(cancellationToken);
            return new ApiResponse<IEnumerable<Project>>
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Ok",
                Data = data
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<Project>>
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
            };
        }
    }

    public Task<ApiResponse<Project?>> CreateAsync(Project entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<Project?>> UpdateAsync(Project entity, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<ApiResponse<object>> DeleteAsync(int Id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
