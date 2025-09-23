using Infrastructure.Models;
using ProjectGamesGuide.Domain.Entities;

namespace ProjectGamesGuide.Application.Interfaces;

public interface IUserGoogleService
{
    Task<ApiResponse<UserLoggedToken>> LoginGoogleAsync(UserLoginGoogle login, CancellationToken cancellationToken);
}
