using Infrastructure.Models;
using ProjectPasswordManager.Application.DTOs;
using ProjectPasswordManager.Application.Interfaces;
using ProjectPasswordManager.Domain.Entities;
using ProjectPasswordManager.Domain.Interfaces;
using Utils;

namespace ProjectPasswordManager.Application.Services;

public class CoreUserService : ICoreUserService
{
    private readonly ICoreUserRepository _coreUserRepository;
    private readonly PasswordUtil _passwordUtil;

    public CoreUserService(ICoreUserRepository coreUserRepository, PasswordUtil passwordUtil)
    {
        _coreUserRepository = coreUserRepository;
        _passwordUtil = passwordUtil;
    }

    public async Task<ApiResponse<CoreUserIV>> RegisterCoreUserPasswordAsync(CoreUserPassword coreUserPassword, CancellationToken cancellationToken)
    {
        try
        {
            var coreUser = await _coreUserRepository.GetCoreUserAsync(
                new CoreUser
                {
                    User_Id = coreUserPassword.CoreUser.User_Id,
                    SqlToken = coreUserPassword.CoreUser.SqlToken
                }, cancellationToken);

            if (coreUser == null)
                return new ApiResponse<CoreUserIV>
                {
                    IsSuccess = false,
                    StatusCode = 401,
                    Message = "Debes Iniciar Sesion."
                };

            if (!string.IsNullOrEmpty(coreUser.HashPM) && !string.IsNullOrEmpty(coreUser.SaltPM))
                return new ApiResponse<CoreUserIV>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = "Ya Tienes una Clave de Encriptación Creada."
                };

            var (hash, salt) = _passwordUtil.HashPassword(coreUserPassword.Password);
            coreUser.HashPM = hash;
            coreUser.SaltPM = salt;

            await _coreUserRepository.RegisterCoreUserPasswordAsync(coreUser, cancellationToken);

            var coreUserIV = new CoreUserIV
            {
                IV = salt,
            };

            return new ApiResponse<CoreUserIV>
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Clave de Encriptación Creada Correctamente.",
                Data = coreUserIV
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<CoreUserIV>
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
            };
        }
    }

    public async Task<ApiResponse<CoreUserIV>> GetCoreUserIVAsync(CoreUserPassword coreUserPassword, CancellationToken cancellationToken)
    {
        try
        {
            var coreUser = await _coreUserRepository.GetCoreUserAsync(
                new CoreUser
                {
                    User_Id = coreUserPassword.CoreUser.User_Id,
                    SqlToken = coreUserPassword.CoreUser.SqlToken
                }, cancellationToken);

            if (coreUser == null)
                return new ApiResponse<CoreUserIV>
                {
                    IsSuccess = false,
                    StatusCode = 401,
                    Message = "Debes Iniciar Sesion."
                };

            if (string.IsNullOrEmpty(coreUser.HashPM) || string.IsNullOrEmpty(coreUser.SaltPM))
                return new ApiResponse<CoreUserIV>
                {
                    IsSuccess = false,
                    StatusCode = 401,
                    Message = "Debes Crear una Clave de Encriptación."
                };

            bool isPasswordOk = _passwordUtil.VerifyPassword(coreUserPassword.Password, coreUser.HashPM, coreUser.SaltPM);

            if (!isPasswordOk)
                return new ApiResponse<CoreUserIV>
                {
                    IsSuccess = false,
                    StatusCode = 401,
                    Message = "Usuario o Contraseña Icorrecta."
                };

            var coreUserIV = new CoreUserIV
            {
                IV = coreUser.SaltPM,
            };

            return new ApiResponse<CoreUserIV>
            {
                IsSuccess = true,
                StatusCode = 200,
                Message = "Autenticación Exitosa.",
                Data = coreUserIV
            };
        }
        catch (Exception ex)
        {
            return new ApiResponse<CoreUserIV>
            {
                IsSuccess = false,
                StatusCode = 500,
                Message = ex.Message,
            };
        }
    }
}
