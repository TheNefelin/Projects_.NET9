using Infrastructure.Models;
using ProjectAuth.Application.DTOs;
using ProjectAuth.Application.Interfaces;
using ProjectAuth.Domain.Entities;
using ProjectAuth.Domain.Interfaces;
using ProjectAuth.Infrastructure.Models;
using ProjectAuth.Infrastructure.Services;
using Utils;

namespace ProjectAuth.Application.Services;

public class AuthUserService : IAuthUserService
{
    private readonly IAuthUserRepository _authUserRepository;
    private readonly PasswordUtil _passwordUtil;
    private readonly JwtTokenUtil _jwtTokenUtil;

    public AuthUserService(IAuthUserRepository authUserRepository, PasswordUtil passwordUtil, JwtTokenUtil jwtTokenUtil)
    {
        _authUserRepository = authUserRepository;
        _passwordUtil = passwordUtil;
        _jwtTokenUtil = jwtTokenUtil;
    }

    public async Task<ApiResponse<AuthUserResponse>> RegisterAsync(AuthUserRegister authUserRegister, CancellationToken cancellationToken)
    {
        try
        {
            if (!authUserRegister.Password1.Equals(authUserRegister.Password2))
                return new ApiResponse<AuthUserResponse>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Las contraseñas no Coinciden.",
                };

            var authUser = await _authUserRepository.GetUserByEmailAsync(authUserRegister.Email, cancellationToken);

            if (authUser != null)
                return new ApiResponse<AuthUserResponse>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Ya estas registrado.",
                };

            var (hash, salt) = _passwordUtil.HashPassword(authUserRegister.Password1);
            var authUserResponse = new AuthUserResponse
            {
                User_Id = Guid.NewGuid()
            };

            var newAuthUser = await _authUserRepository.CreateUserAsync(
                new AuthUser
                {
                    User_Id = authUserResponse.User_Id,
                    Email = authUserRegister.Email,
                    HashLogin = hash,
                    SaltLogin = salt,
                },
                cancellationToken);

            return new ApiResponse<AuthUserResponse>
            {
                IsSuccess = newAuthUser.IsSuccess,
                StatusCode = newAuthUser.StatusCode,
                Message = newAuthUser.Message,
                Data = authUserResponse
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<AuthUserResponse>
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.ToString(),
            };
        }
    }

    public async Task<ApiResponse<AuthUserLogged>> LoginAsync(AuthUserLogin authUserLogin, JwtConfig jwtConfig, CancellationToken cancellationToken)
    {
        try
        {
            var authUser = await _authUserRepository.GetUserByEmailAsync(authUserLogin.Email, cancellationToken);

            if (authUser == null)
                return new ApiResponse<AuthUserLogged>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Usuario o Contraseña Incorrecta.",
                };

            var isValidPassword = _passwordUtil.VerifyPassword(authUserLogin.Password, authUser.HashLogin, authUser.SaltLogin);

            if (!isValidPassword)
                return new ApiResponse<AuthUserLogged>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Usuario o Contraseña Incorrecta.",
                };

            var jwtUser = new JwtUser
            {
                User_Id = authUser.User_Id,
                Email = authUser.Email,
                Role = authUser.Role
            };

            var sqlToken = await _authUserRepository.NewSqlToken(authUser.Email, cancellationToken);
            var apiToken = _jwtTokenUtil.GenerateJwtToken(jwtUser, jwtConfig);

            var authUserLogged = new AuthUserLogged
            {
                User_Id = authUser.User_Id,
                Role = authUser.Role!,
                ExpireMin = jwtConfig.ExpireMin,
                SqlToken = sqlToken!,
                ApiToken = apiToken
            };

            return new ApiResponse<AuthUserLogged>
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Login Exitoso.",
                Data = authUserLogged
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<AuthUserLogged>
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
            };
        }
    }
}
