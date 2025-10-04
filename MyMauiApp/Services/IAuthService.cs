using MyMauiApp.Models;

namespace MyMauiApp.Services;

public interface IAuthService
{
    Task<ApiResponse<AuthUserLogged>> LoginAsync(AuthUserLogin credentials);
    Task LogoutAsync();
}
