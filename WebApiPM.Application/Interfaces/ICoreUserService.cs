using WebApiPM.Application.Common;
using WebApiPM.Application.DTOs;

namespace WebApiPM.Application.Interfaces;

public interface ICoreUserService
{
    Task<ApiResponse<CoreUserIV>> RegisterCoreUserPasswordAsync(Guid userId, CoreUserPassword coreUserRequest, CancellationToken cancellationToken);
    Task<ApiResponse<CoreUserIV>> GetCoreUserIVAsync(Guid userId, CoreUserPassword coreUserRequest, CancellationToken cancellationToken);
}