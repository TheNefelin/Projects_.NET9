using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using ProjectAuth.Application.DTOs;
using ProjectAuth.Application.Interfaces;
using ProjectAuth.Infrastructure.Models;
using WebApi.Filters;

namespace WebApi.Controllers;

[Route("api/auth")]
[ApiController]
[ServiceFilter(typeof(ApiKeyFilter))]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IAuthUserService _authUserService;

    public AuthController(IAuthUserService authUserService, IConfiguration configuration)
    {
        _authUserService = authUserService;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<AuthUserResponse>>> Register(AuthUserRegister authUserRegister, CancellationToken cancelationToken)
    {
        var apiResult = await _authUserService.RegisterAsync(authUserRegister, cancelationToken);
        return StatusCode(apiResult.StatusCode, apiResult);
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<AuthUserLogged>>> Login(AuthUserLogin authUserLogin, CancellationToken cancelationToken)
    {
        var jwtConfig = new JwtConfig()
        {
            Key = _configuration["Jwt:Key"]!,
            Issuer = _configuration["Jwt:Issuer"]!,
            Audience = _configuration["Jwt:Audience"]!,
            Subject = _configuration["JWT:Subject"]!,
            ExpireMin = _configuration["JWT:ExpireMin"]!
        };

        var apiResult = await _authUserService.LoginAsync(authUserLogin, jwtConfig, cancelationToken);
        return StatusCode(apiResult.StatusCode, apiResult);
    }
}
