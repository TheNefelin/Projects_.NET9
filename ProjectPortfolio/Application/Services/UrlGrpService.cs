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

    public async Task<ApiResponse<UrlGrp?>> CreateAsync(UrlGrp entity, CancellationToken cancellationToken)
    {
        try
        {
            var id = await _repository.CreateAsync(entity, cancellationToken);
            entity.Id = id;

            return new ApiResponse<UrlGrp?>
            {
                IsSuccess = true,
                StatusCode = 201,
                Message = "Created successfully",
                Data = entity
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<UrlGrp?>
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
            };
        }
    }

    public async Task<ApiResponse<UrlGrp?>> UpdateAsync(UrlGrp entity, CancellationToken cancellationToken)
    {
        try
        {
            var rowsAffected = await _repository.UpdateAsync(entity, cancellationToken);

            if (rowsAffected == 0)
                return new ApiResponse<UrlGrp?>
                {
                    IsSuccess = false,
                    StatusCode = 401,
                    Message = "Update faild"
                };

            return new ApiResponse<UrlGrp?>
            {
                IsSuccess = true,
                StatusCode = 201,
                Message = "Updated successfully",
                Data = entity
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<UrlGrp?>
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
            };
        }
    }

    public async Task<ApiResponse<object>> DeleteAsync(int Id, CancellationToken cancellationToken)
    {
        try
        {
            var rowsAffected = await _repository.DeleteAsync(Id, cancellationToken);

            if (rowsAffected == 0)
                return new ApiResponse<object>
                {
                    IsSuccess = false,
                    StatusCode = 401,
                    Message = "Delete faild"
                };

            return new ApiResponse<object>
            {
                IsSuccess = true,
                StatusCode = 200, //204,
                Message = "Deleted successfully",
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<object>
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
            };
        }
    }
}
