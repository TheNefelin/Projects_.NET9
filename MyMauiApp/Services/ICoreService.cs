using MyMauiApp.Models;

namespace MyMauiApp.Services;

public interface ICoreService
{
    Task<ApiResponse<CoreUserIV>> RegisterPasswordAsync(string password);
    Task<ApiResponse<CoreUserIV>> GetCoreUserIVAsync(string password);
    Task<ApiResponse<IEnumerable<CoreUserData>>> GetAllCore();
    Task<ApiResponse<CoreUserData>> InsertCore(CoreUserData coreUserData);
    Task<ApiResponse<CoreUserData>> UpdateCore(CoreUserData coreUserData);
    Task<ApiResponse<object>> DeleteCore(Guid DataId);
}
