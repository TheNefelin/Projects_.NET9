using Infrastructure.Models;
using ProjectPasswordManager.Application.DTOs;
using ProjectPasswordManager.Domain.Entities;

namespace ProjectPasswordManager.Application.Interfaces;

public interface ICoreDataService
{
    Task<ApiResponse<IEnumerable<CoreData>>> GetAllAsync(CoreUserRequest coreUser, CancellationToken cancellationToken);
    Task<ApiResponse<CoreData>> InsertAsync(CoreDataRequest coreData, CancellationToken cancellationToken);
    Task<ApiResponse<CoreData>> UpdateAsync(CoreDataRequest coreData, CancellationToken cancellationToken);
    Task<ApiResponse<object>> DeleteAsync(CoreDataDelete coreDataDelete, CancellationToken cancellationToken);
}
