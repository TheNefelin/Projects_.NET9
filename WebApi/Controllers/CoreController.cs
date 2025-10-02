using Infrastructure.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectPasswordManager.Application.DTOs;
using ProjectPasswordManager.Application.Interfaces;
using ProjectPasswordManager.Domain.Entities;
using WebApi.Filters;

namespace WebApi.Controllers;

[Route("api/core")]
[ApiController]
[ServiceFilter(typeof(ApiKeyFilter))]
[Authorize]
public class CoreController : ControllerBase
{
    private readonly ICoreDataService _coreService;
    private readonly ICoreUserService _coreUserService;

    public CoreController(ICoreDataService coreService, ICoreUserService coreUserService)
    {
        _coreService = coreService;
        _coreUserService = coreUserService;
    }

    [HttpPost("register-password")]
    public async Task<ActionResult<ApiResponse<CoreUserIV>>> RegisterCoreUserPassword(CoreUserPassword coreUserRequest, CancellationToken cancelationToken)
    {
        var apiResult = await _coreUserService.RegisterCoreUserPasswordAsync(coreUserRequest, cancelationToken);
        return StatusCode(apiResult.StatusCode, apiResult);
    }

    [HttpPost("get-iv")]
    public async Task<ActionResult<ApiResponse<CoreUserIV>>> GetCoreUserIV(CoreUserPassword coreUserRequest, CancellationToken cancelationToken)
    {
        var apiResult = await _coreUserService.GetCoreUserIVAsync(coreUserRequest, cancelationToken);
        return StatusCode(apiResult.StatusCode, apiResult);
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<CoreData>>>> GetAllCore([FromQuery] CoreUserRequest coreUserRequest, CancellationToken cancelationToken)
    {
        //Id_User = "3fa85f64-5717-4562-b3fc-2c963f66afa6";
        var apiResult = await _coreService.GetAllAsync(coreUserRequest, cancelationToken);
        return StatusCode(apiResult.StatusCode, apiResult);
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<CoreData>>> InsertCore(CoreDataRequest coreDataRequest, CancellationToken cancelationToken)
    {
        var apiResult = await _coreService.InsertAsync(coreDataRequest, cancelationToken);
        return StatusCode(apiResult.StatusCode, apiResult);
    }

    [HttpPut]
    public async Task<ActionResult<ApiResponse<CoreData>>> UpdateCore(CoreDataRequest coreDataRequest, CancellationToken cancelationToken)
    {
        var apiResult = await _coreService.UpdateAsync(coreDataRequest, cancelationToken);
        return StatusCode(apiResult.StatusCode, apiResult);
    }

    [HttpDelete]
    public async Task<ActionResult<ApiResponse<object>>> DeleteCore(CoreDataDelete coreDataDelete, CancellationToken cancelationToken)
    {
        var apiResult = await _coreService.DeleteAsync(coreDataDelete, cancelationToken);
        return StatusCode(apiResult.StatusCode, apiResult);
    }
}
