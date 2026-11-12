using WebApiPM.Application.Common;
using WebApiPM.Application.DTOs;
using WebApiPM.Application.Interfaces;
using WebApiPM.Domain.Entities;
using WebApiPM.Domain.Interfaces;
using WebApiPM.Domain.Models;

namespace WebApiPM.Application.Services;

public class AuthUserService : IAuthUserService
{
    private const string TooManyLoginAttempts = "Demasiados intentos fallidos de inicio de sesión. Intenta nuevamente más tarde.";

    private readonly IAuthUserRepository _authUserRepository;
    private readonly IMaeConfigRepository _maeConfigRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IAuthTokenService _authTokenService;
    private readonly IIpLockoutService _loginLockoutService;

    public AuthUserService(
        IAuthUserRepository authUserRepository,
        IMaeConfigRepository maeConfigRepository,
        IPasswordHasher passwordHasher,
        IAuthTokenService authTokenService,
        IIpLockoutService loginLockoutService)
    {
        _authUserRepository = authUserRepository;
        _maeConfigRepository = maeConfigRepository;
        _passwordHasher = passwordHasher;
        _authTokenService = authTokenService;
        _loginLockoutService = loginLockoutService;
    }

    public async Task<ApiResponse<AuthUserResponse>> RegisterAsync(AuthUserRegister authUserRegister, CancellationToken cancellationToken)
    {
        if (!authUserRegister.Password1.Equals(authUserRegister.Password2))
            return ApiResponse.Failure<AuthUserResponse>(400, "Las contraseñas no coinciden.");

        // El flag vivia dentro del stored procedure Auth_Register; con el
        // INSERT parametrizado la comprobacion queda en el servicio.
        if (!await _maeConfigRepository.IsRegistrationEnabledAsync(cancellationToken))
            return ApiResponse.Failure<AuthUserResponse>(403, "El Servicio de Registro No Esta Disponible");

        var (hash, salt) = _passwordHasher.HashPassword(authUserRegister.Password1);
        var authUser = new AuthUser
        {
            User_Id = Guid.NewGuid(),
            Email = authUserRegister.Email,
            HashLogin = hash,
            SaltLogin = salt
        };

        var result = await _authUserRepository.CreateUserAsync(authUser, cancellationToken);

        if (result == UserCreationStatus.EmailAlreadyExists)
            return ApiResponse.Failure<AuthUserResponse>(409, "Ya estás registrado.");

        return ApiResponse.Success(
            new AuthUserResponse { User_Id = authUser.User_Id },
            "Usuario Registrado Correctamente",
            201);
    }

    public async Task<ApiResponse<AuthUserLogged>> LoginAsync(AuthUserLogin authUserLogin, string ipAddress, CancellationToken cancellationToken)
    {
        if (_loginLockoutService.IsBlocked(ipAddress))
            return ApiResponse.Failure<AuthUserLogged>(429, TooManyLoginAttempts);

        var authUser = await _authUserRepository.GetUserByEmailAsync(authUserLogin.Email, cancellationToken);

        if (authUser == null)
        {
            _loginLockoutService.RegisterFailure(ipAddress);
            return ApiResponse.Failure<AuthUserLogged>(401, "Usuario o contraseña incorrecta.");
        }

        if (!_passwordHasher.VerifyPassword(authUserLogin.Password, authUser.HashLogin, authUser.SaltLogin))
        {
            _loginLockoutService.RegisterFailure(ipAddress);
            return ApiResponse.Failure<AuthUserLogged>(401, "Usuario o contraseña incorrecta.");
        }

        _loginLockoutService.Reset(ipAddress);

        var sqlToken = await _authUserRepository.NewSqlToken(authUser.Email, cancellationToken);
        var token = _authTokenService.GenerateToken(authUser);

        return ApiResponse.Success(
            new AuthUserLogged
            {
                User_Id = authUser.User_Id,
                SqlToken = sqlToken,
                Role = authUser.Role ?? "USER",
                ExpireMin = token.ExpireMin.ToString(),
                ApiToken = token.Token
            },
            "Login exitoso.");
    }
}