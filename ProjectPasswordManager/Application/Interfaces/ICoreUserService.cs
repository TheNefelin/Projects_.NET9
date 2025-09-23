using Infrastructure.Models;
using ProjectPasswordManager.Application.DTOs;

namespace ProjectPasswordManager.Application.Interfaces;

public interface ICoreUserService
{
    Task<ApiResponse<CoreUserIV>> RegisterCoreUserPasswordAsync(CoreUserPassword coreUserRequest, CancellationToken cancellationToken);
    Task<ApiResponse<CoreUserIV>> GetCoreUserIVAsync(CoreUserPassword coreUserRequest, CancellationToken cancellationToken);
}
