using Infrastructure.Models;
using ProjectGamesGuide.Application.DTOs;
using ProjectGamesGuide.Domain.Entities;

namespace ProjectGamesGuide.Application.Interfaces;

public interface IUserAdventureService
{
    Task<ApiResponse<IEnumerable<UserAdventure>>> GetAllByUserIdAsync(Guid Id_User, CancellationToken cancellationToken);
    Task<ApiResponse<object>> UpdateAsync(UserAdventureRequest request, CancellationToken cancellationToken);
}
