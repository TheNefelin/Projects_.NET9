using WebApiPM.Application.DTOs;

namespace WebApiPM.Application.Interfaces;

public interface IAuthUserService
{
    Task<AuthUserResponse> RegisterAsync(AuthUserRegister authUserRegister, CancellationToken cancellationToken);
    Task<AuthUserLogged> LoginAsync(AuthUserLogin authUserLogin, string ipAddress, CancellationToken cancellationToken);
}
