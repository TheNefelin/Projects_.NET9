using WebApiPM.Application.Common;
using WebApiPM.Application.DTOs;
using WebApiPM.Application.Interfaces;
using WebApiPM.Domain.Entities;
using WebApiPM.Domain.Interfaces;

namespace WebApiPM.Application.Services;

public class CoreUserService : ICoreUserService
{
    private const int SaltSize = 16;

    private readonly ICoreUserRepository _coreUserRepository;
    private readonly IPasswordHasher _passwordHasher;

    public CoreUserService(ICoreUserRepository coreUserRepository, IPasswordHasher passwordHasher)
    {
        _coreUserRepository = coreUserRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<ApiResponse<CoreUserIV>> RegisterCoreUserPasswordAsync(Guid userId, CoreUserPasswordCreate coreUserPassword, CancellationToken cancellationToken)
    {
        var coreUser = await _coreUserRepository.GetCoreUserAsync(
            new CoreUser
            {
                User_Id = userId,
                SqlToken = coreUserPassword.CoreUser.SqlToken
            },
            cancellationToken);

        if (coreUser == null)
            return ApiResponse.Failure<CoreUserIV>(401, "Debes iniciar sesión.");

        if (!string.IsNullOrEmpty(coreUser.HashPM) && !string.IsNullOrEmpty(coreUser.SaltPM))
            return ApiResponse.Failure<CoreUserIV>(400, "Ya tienes una clave de encriptación creada.");

        var (hash, salt) = _passwordHasher.HashPassword(coreUserPassword.Password);
        coreUser.HashPM = hash;
        coreUser.SaltPM = salt;

        await _coreUserRepository.RegisterCoreUserPasswordAsync(coreUser, cancellationToken);

        return ApiResponse.Success(
            new CoreUserIV { IV = salt },
            "Clave de encriptación creada correctamente.");
    }

    public async Task<ApiResponse<CoreUserIV>> GetCoreUserIVAsync(Guid userId, CoreUserPassword coreUserPassword, CancellationToken cancellationToken)
    {
        var coreUser = await _coreUserRepository.GetCoreUserAsync(
            new CoreUser
            {
                User_Id = userId,
                SqlToken = coreUserPassword.CoreUser.SqlToken
            },
            cancellationToken);

        if (coreUser == null)
            return ApiResponse.Failure<CoreUserIV>(401, "Debes iniciar sesión.");

        if (string.IsNullOrEmpty(coreUser.HashPM) || string.IsNullOrEmpty(coreUser.SaltPM))
            return ApiResponse.Failure<CoreUserIV>(401, "Debes crear una clave de encriptación.");

        if (!_passwordHasher.VerifyPassword(coreUserPassword.Password, coreUser.HashPM, coreUser.SaltPM))
            return ApiResponse.Failure<CoreUserIV>(401, "Usuario o contraseña incorrecta.");

        return ApiResponse.Success(
            new CoreUserIV { IV = coreUser.SaltPM },
            "Autenticación exitosa.");
    }

    public async Task<ApiResponse<CoreUserChangeResponse>> ChangeCorePasswordAsync(Guid userId, ChangeCorePasswordRequest request, CancellationToken cancellationToken)
    {
        var coreUser = await _coreUserRepository.GetCoreUserAsync(
            new CoreUser
            {
                User_Id = userId,
                SqlToken = request.CoreUser.SqlToken
            },
            cancellationToken);

        if (coreUser == null)
            return ApiResponse.Failure<CoreUserChangeResponse>(401, "Debes iniciar sesión.");

        if (string.IsNullOrEmpty(coreUser.HashPM) || string.IsNullOrEmpty(coreUser.SaltPM))
            return ApiResponse.Failure<CoreUserChangeResponse>(401, "Debes crear una clave de encriptación.");

        if (!_passwordHasher.VerifyPassword(request.OldPassword, coreUser.HashPM, coreUser.SaltPM))
            return ApiResponse.Failure<CoreUserChangeResponse>(401, "Usuario o contraseña incorrecta.");

        // La sal/IV de 16 bytes la genera el cliente (dueno del cifrado) para
        // poder re-cifrar antes del cambio; aqui solo se valida que sea material
        // valido antes de usarla en el hash PBKDF2.
        if (!ValidNewSalt(request.Salt, out string? saltError))
            return ApiResponse.Failure<CoreUserChangeResponse>(400, saltError!);

        var newHash = _passwordHasher.HashPassword(request.NewPassword, request.Salt);
        var newSqlToken = Guid.NewGuid();

        // La atomicidad (swap de clave + reemplazo de datos, todo o nada) vive
        // en una unica transaccion del repositorio: es el contrato que evita
        // perder datos si el request falla a mitad.
        await _coreUserRepository.ChangeCorePasswordAsync(
            userId,
            newHash,
            request.Salt,
            newSqlToken,
            request.Records.Select(ToEntity),
            cancellationToken);

        return ApiResponse.Success(
            new CoreUserChangeResponse { IV = request.Salt, SqlToken = newSqlToken },
            "Clave maestra actualizada correctamente.");
    }

    private static bool ValidNewSalt(string salt, out string? error)
    {
        if (string.IsNullOrEmpty(salt))
        {
            error = "La nueva sal es obligatoria.";
            return false;
        }

        try
        {
            byte[] bytes = Convert.FromBase64String(salt);
            if (bytes.Length != SaltSize)
            {
                error = "La nueva sal debe ser de 16 bytes.";
                return false;
            }
        }
        catch (FormatException)
        {
            error = "La nueva sal no es base64 válido.";
            return false;
        }

        error = null;
        return true;
    }

    private static CoreData ToEntity(CoreDataReplacement replacement)
    {
        return new CoreData
        {
            Data_Id = replacement.Data_Id,
            Data01 = replacement.Data01,
            Data02 = replacement.Data02,
            Data03 = replacement.Data03
        };
    }
}
