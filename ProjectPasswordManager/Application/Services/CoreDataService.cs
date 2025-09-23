using Infrastructure.Models;
using ProjectPasswordManager.Application.DTOs;
using ProjectPasswordManager.Application.Interfaces;
using ProjectPasswordManager.Domain.Entities;
using ProjectPasswordManager.Domain.Interfaces;

namespace ProjectPasswordManager.Application.Services;

public class CoreDataService : ICoreDataService
{
    private readonly ICoreDataRepository _coreDataRepository;
    private readonly ICoreUserRepository _coreUserRepository;

    public CoreDataService(ICoreDataRepository coreDataRepository, ICoreUserRepository coreUserRepository)
    {
        _coreDataRepository = coreDataRepository;
        _coreUserRepository = coreUserRepository;
    }

    public async Task<ApiResponse<IEnumerable<CoreData>>> GetAllAsync(CoreUserRequest coreUserRequest, CancellationToken cancellationToken)
    {
        try
        {
            var coreUser = await _coreUserRepository.GetCoreUserAsync(
                new CoreUser
                {
                    User_Id = coreUserRequest.User_Id,
                    SqlToken = coreUserRequest.SqlToken
                }, cancellationToken);

            if (coreUser == null)
                return new ApiResponse<IEnumerable<CoreData>>
                {
                    IsSuccess = false,
                    StatusCode = 401,
                    Message = "Debes Iniciar Sesion."
                };

            var coreDatas = await _coreDataRepository.GetAllAsync(
                new CoreData
                {
                    User_Id = coreUser.User_Id
                }, cancellationToken);

            return new ApiResponse<IEnumerable<CoreData>>
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Ok",
                Data = coreDatas
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<CoreData>>
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
            };
        }
    }

    public async Task<ApiResponse<CoreData>> InsertAsync(CoreDataRequest coreDataRequest, CancellationToken cancellationToken)
    {
        try
        {
            var coreUser = await _coreUserRepository.GetCoreUserAsync(
                new CoreUser
                {
                    User_Id = coreDataRequest.CoreUser.User_Id,
                    SqlToken = coreDataRequest.CoreUser.SqlToken
                }, cancellationToken);

            if (coreUser == null)
                return new ApiResponse<CoreData>
                {
                    IsSuccess = false,
                    StatusCode = 401,
                    Message = "Debes Iniciar Sesion."
                };

            var coreData = await _coreDataRepository.InsertAsync(
                new CoreData
                {
                    Data01 = coreDataRequest.Data01,
                    Data02 = coreDataRequest.Data02,
                    Data03 = coreDataRequest.Data03,
                    User_Id = coreUser.User_Id
                }, cancellationToken);

            return new ApiResponse<CoreData>
            {
                IsSuccess = true,
                StatusCode = 201,
                Message = "Created",
                Data = coreData
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<CoreData>
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
            };
        }
    }

    public async Task<ApiResponse<CoreData>> UpdateAsync(CoreDataRequest coreDataRequest, CancellationToken cancellationToken)
    {
        try
        {
            var coreUser = await _coreUserRepository.GetCoreUserAsync(
                new CoreUser
                {
                    User_Id = coreDataRequest.CoreUser.User_Id,
                    SqlToken = coreDataRequest.CoreUser.SqlToken
                }, cancellationToken);

            if (coreUser == null)
                return new ApiResponse<CoreData>
                {
                    IsSuccess = false,
                    StatusCode = 401,
                    Message = "Debes Iniciar Sesion."
                };

            var coreData = await _coreDataRepository.UpdateAsync(
                new CoreData
                {
                    Data_Id = coreDataRequest.Data_Id,
                    Data01 = coreDataRequest.Data01,
                    Data02 = coreDataRequest.Data02,
                    Data03 = coreDataRequest.Data03,
                    User_Id = coreDataRequest.CoreUser.User_Id
                }, cancellationToken);
            return new ApiResponse<CoreData>
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Ok",
                Data = coreData
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<CoreData>
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
            };
        }
    }

    public async Task<ApiResponse<object>> DeleteAsync(CoreDataDelete coreDataDelete, CancellationToken cancellationToken)
    {
        try
        {
            var coreUser = await _coreUserRepository.GetCoreUserAsync(
                new CoreUser
                {
                    User_Id = coreDataDelete.CoreUser.User_Id,
                    SqlToken = coreDataDelete.CoreUser.SqlToken
                }, cancellationToken);

            if (coreUser == null)
                return new ApiResponse<object>
                {
                    IsSuccess = false,
                    StatusCode = 401,
                    Message = "Debes Iniciar Sesion."
                };

            await _coreDataRepository.DeleteAsync(
                new CoreData
                {
                    Data_Id = coreDataDelete.Data_Id,
                    User_Id = coreDataDelete.CoreUser.User_Id,
                }, cancellationToken);

            return new ApiResponse<object>
            {
                IsSuccess = true,
                StatusCode = 204,
                Message = "No Content"
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
