using WebApiPM.Application.Common;
using WebApiPM.Application.DTOs;

namespace WebApiPM.Application.Interfaces;

public interface IAuthUserService
{
    Task<ApiResponse<AuthUserResponse>> RegisterAsync(AuthUserRegister authUserRegister, CancellationToken cancellationToken);
    Task<ApiResponse<AuthUserLogged>> LoginAsync(AuthUserLogin authUserLogin, string ipAddress, CancellationToken cancellationToken);
}