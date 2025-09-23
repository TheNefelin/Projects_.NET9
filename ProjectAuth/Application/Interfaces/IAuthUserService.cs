using Infrastructure.Models;
using ProjectAuth.Application.DTOs;
using ProjectAuth.Infrastructure.Models;

namespace ProjectAuth.Application.Interfaces;

public interface IAuthUserService
{
    Task<ApiResponse<AuthUserResponse>> RegisterAsync(AuthUserRegister authUserRegister, CancellationToken cancellationToken);
    Task<ApiResponse<AuthUserLogged>> LoginAsync(AuthUserLogin authUserLogin, JwtConfig jwtConfig, CancellationToken cancellationToken);
}
