using MyMauiApp.Models;

namespace MyMauiApp.Services;

public interface ICoreService
{
    Task<ApiResponse<CoreUserIV>> RegisterPasswordAsync(CorePasswordRequest corePasswordRequest);
    Task<ApiResponse<CoreUserIV>> GetCoreUserIVAsync(CorePasswordRequest corePasswordRequest);
    Task<ApiResponse<IEnumerable<CoreUserData>>> GetAllCore(CoreUserRequest coreUserRequest);
    Task<ApiResponse<CoreUserData>> InsertCore(CoreDataRequest coreDataRequest);
    Task<ApiResponse<CoreUserData>> UpdateCore(CoreDataRequest coreDataRequest);
    Task<ApiResponse<object>> DeleteCore(CoreDataDelete coreDataDelete);
}
