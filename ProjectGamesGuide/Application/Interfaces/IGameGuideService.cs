using Infrastructure.Models;
using ProjectGamesGuide.Application.DTOs;

namespace ProjectGamesGuide.Application.Interfaces;

public interface IGameGuideService
{
    Task<ApiResponse<IEnumerable<GameResponse>>> GetAllAsync(Guid Id_User, CancellationToken cancellationToken);
}
