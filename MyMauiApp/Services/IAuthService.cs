using MyMauiApp.Models;

namespace MyMauiApp.Services;

public interface IAuthService
{
    Task<ApiResponse<AuthUserLogged>> LoginAsync(AuthUserLogin credentials);
    Task LogoutAsync();
    Task<bool> IsAuthenticatedAsync();
    Task<string?> GetJwtTokenAsync();
    Task<string?> GetSqlTokenAsync();
    Task<int> GetExpireMinAsync();
    Task<string?> GetUserIdAsync();
    Task<string?> GetRoleAsync();
}
