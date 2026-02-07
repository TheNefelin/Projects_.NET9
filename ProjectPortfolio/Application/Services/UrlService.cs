using Infrastructure.Models;
using Microsoft.VisualBasic;
using ProjectPortfolio.Application.Interfaces;
using ProjectPortfolio.Domain.Entities;
using ProjectPortfolio.Domain.Interfaces;
using static Dapper.SqlMapper;
using static System.Runtime.InteropServices.JavaScript.JSType;

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

    public async Task<ApiResponse<Url?>> CreateAsync(Url entity, CancellationToken cancellationToken)
    {
        try
        {
            var id = await _repository.CreateAsync(entity, cancellationToken);
            entity.Id = id;

            return new ApiResponse<Url?>
            { 
                IsSuccess = true,
                StatusCode = 201,
                Message = "Created successfully",
                Data = entity
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<Url?>
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
            };
        }
    }

    public async Task<ApiResponse<Url?>> UpdateAsync(Url entity, CancellationToken cancellationToken)
    {
        try
        {
            var rowsAffected = await _repository.UpdateAsync(entity, cancellationToken);
           
            if (rowsAffected == 0)
                return new ApiResponse<Url?>
                {
                    IsSuccess = false,
                    StatusCode = 401,
                    Message = "Update faild"
                };

            return new ApiResponse<Url?>
            {
                IsSuccess = true,
                StatusCode = 201,
                Message = "Updated successfully",
                Data = entity
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<Url?>
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
