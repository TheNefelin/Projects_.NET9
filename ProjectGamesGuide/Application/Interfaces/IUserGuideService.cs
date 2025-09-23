using Infrastructure.Models;
using ProjectGamesGuide.Application.DTOs;
using ProjectGamesGuide.Domain.Entities;

namespace ProjectGamesGuide.Application.Interfaces;

public interface IUserGuideService
{
    Task<ApiResponse<IEnumerable<UserGuide>>> GetAllByUserIdAsync(Guid Id_User, CancellationToken cancellationToken);
    Task<ApiResponse<object>> UpdateAsync(UserGuideRequest request, CancellationToken cancellationToken);
}
