using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Security.Claims;
using WebApi.Filters;
using WebApi.Helpers;
using WebApiPM.Application.Common;
using WebApiPM.Application.DTOs;
using WebApiPM.Application.Interfaces;

namespace WebApi.Controllers;

[Route("api/core")]
[ApiController]
[ServiceFilter(typeof(ApiKeyFilter))]
[Authorize]
[EnableRateLimiting("client_25_per_minute")]
public class CoreController : ControllerBase
{
    private const string SqlTokenHeaderName = "SqlToken";

    private readonly ICoreDataService _coreService;
    private readonly ICoreUserService _coreUserService;
    private readonly IIpLockoutService _corePasswordLockout;
    private readonly ILogger<CoreController> _logger;

    public CoreController(
        ICoreDataService coreService,
        ICoreUserService coreUserService,
        IIpLockoutService corePasswordLockout,
        ILogger<CoreController> logger)
    {
        _coreService = coreService;
        _coreUserService = coreUserService;
        _corePasswordLockout = corePasswordLockout;
        _logger = logger;
    }

    [HttpPost("register-password")]
    public async Task<ActionResult<ApiResponse<CoreUserIV>>> RegisterCoreUserPassword(CoreUserPasswordCreate coreUserRequest, CancellationToken cancellationToken)
    {
        if (TryGetUserId(out var userId) is ActionResult unauthorized)
            return unauthorized;

        var apiResult = await _coreUserService.RegisterCoreUserPasswordAsync(userId, coreUserRequest, cancellationToken);
        return StatusCode(apiResult.StatusCode, apiResult);
    }

    [HttpPost("change-password")]
    public async Task<ActionResult<ApiResponse<CoreUserChangeResponse>>> ChangeCoreUserPassword(ChangeCorePasswordRequest coreUserRequest, CancellationToken cancellationToken)
    {
        if (TryGetUserId(out var userId) is ActionResult unauthorized)
            return unauthorized;

        var clientIp = ClientIpResolver.Resolve(HttpContext);

        if (_corePasswordLockout.IsBlocked(clientIp))
            return StatusCode(429, ApiResponse.Failure<object>(429, "Demasiados intentos fallidos de clave maestra. Intenta nuevamente más tarde."));

        var apiResult = await _coreUserService.ChangeCorePasswordAsync(userId, coreUserRequest, cancellationToken);

        if (apiResult.IsSuccess)
        {
            _corePasswordLockout.Reset(clientIp);
        }
        else if (apiResult.StatusCode == 401)
        {
            _corePasswordLockout.RegisterFailure(clientIp);

            if (_corePasswordLockout.IsBlocked(clientIp))
                _logger.LogWarning("IP {Ip} bloqueada por exceso de intentos fallidos de clave maestra.", clientIp);
        }

        return StatusCode(apiResult.StatusCode, apiResult);
    }

    [HttpPost("get-iv")]
    public async Task<ActionResult<ApiResponse<CoreUserIV>>> GetCoreUserIV(CoreUserPassword coreUserRequest, CancellationToken cancellationToken)
    {
        if (TryGetUserId(out var userId) is ActionResult unauthorized)
            return unauthorized;

        var clientIp = ClientIpResolver.Resolve(HttpContext);

        if (_corePasswordLockout.IsBlocked(clientIp))
            return StatusCode(429, ApiResponse.Failure<object>(429, "Demasiados intentos fallidos de clave maestra. Intenta nuevamente más tarde."));

        var apiResult = await _coreUserService.GetCoreUserIVAsync(userId, coreUserRequest, cancellationToken);

        if (apiResult.IsSuccess)
        {
            _corePasswordLockout.Reset(clientIp);
        }
        else if (apiResult.StatusCode == 401)
        {
            _corePasswordLockout.RegisterFailure(clientIp);

            if (_corePasswordLockout.IsBlocked(clientIp))
                _logger.LogWarning("IP {Ip} bloqueada por exceso de intentos fallidos de clave maestra.", clientIp);
        }

        return StatusCode(apiResult.StatusCode, apiResult);
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<CoreDataResponse>>>> GetAllCore(
        [FromHeader(Name = SqlTokenHeaderName)] string? sqlToken,
        CancellationToken cancellationToken)
    {
        if (TryGetUserId(out var userId) is ActionResult unauthorized)
            return unauthorized;

        // El token de sesion viaja en el header, nunca en la query string: en la
        // URL quedaba expuesto en logs, proxies e historial del navegador.
        if (!Guid.TryParse(sqlToken, out var sqlTokenValue))
            return StatusCode(401, ApiResponse.Failure<object>(401, "Debes iniciar sesión."));

        var apiResult = await _coreService.GetAllAsync(userId, sqlTokenValue, cancellationToken);
        return StatusCode(apiResult.StatusCode, apiResult);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CoreDataResponse>>> InsertCore(CoreDataRequest coreDataRequest, CancellationToken cancellationToken)
    {
        if (TryGetUserId(out var userId) is ActionResult unauthorized)
            return unauthorized;

        var apiResult = await _coreService.InsertAsync(userId, coreDataRequest, cancellationToken);
        return StatusCode(apiResult.StatusCode, apiResult);
    }

    [HttpPut]
    public async Task<ActionResult<ApiResponse<CoreDataResponse>>> UpdateCore(CoreDataRequest coreDataRequest, CancellationToken cancellationToken)
    {
        if (TryGetUserId(out var userId) is ActionResult unauthorized)
            return unauthorized;

        var apiResult = await _coreService.UpdateAsync(userId, coreDataRequest, cancellationToken);
        return StatusCode(apiResult.StatusCode, apiResult);
    }

    [HttpDelete]
    public async Task<ActionResult<ApiResponse<object>>> DeleteCore(CoreDataDelete coreDataDelete, CancellationToken cancellationToken)
    {
        if (TryGetUserId(out var userId) is ActionResult unauthorized)
            return unauthorized;

        var apiResult = await _coreService.DeleteAsync(userId, coreDataDelete, cancellationToken);
        return StatusCode(apiResult.StatusCode, apiResult);
    }

    private ActionResult? TryGetUserId(out Guid userId)
    {
        var sub = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (Guid.TryParse(sub, out userId))
            return null;

        return StatusCode(401, ApiResponse.Failure<object>(401, "No autorizado."));
    }
}
