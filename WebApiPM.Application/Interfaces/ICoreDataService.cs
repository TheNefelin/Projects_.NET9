using WebApiPM.Application.Common;
using WebApiPM.Application.DTOs;

namespace WebApiPM.Application.Interfaces;

public interface ICoreDataService
{
    Task<ApiResponse<IEnumerable<CoreDataResponse>>> GetAllAsync(Guid userId, CoreUserRequest coreUser, CancellationToken cancellationToken);
    Task<ApiResponse<CoreDataResponse>> InsertAsync(Guid userId, CoreDataRequest coreData, CancellationToken cancellationToken);
    Task<ApiResponse<CoreDataResponse>> UpdateAsync(Guid userId, CoreDataRequest coreData, CancellationToken cancellationToken);
    Task<ApiResponse<object>> DeleteAsync(Guid userId, CoreDataDelete coreDataDelete, CancellationToken cancellationToken);
}