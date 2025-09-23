using Infrastructure.Models;
using ProjectGamesGuide.Domain.Entities;

namespace ProjectGamesGuide.Domain.Interfaces;

public interface IUserGoogleRepository
{
    Task<SqlGoogleResponse> LoginGoogleAsync(UserLoginGoogle login, CancellationToken cancellationToken);
    Task<UserLoginGoogle?> ValidateLoginAsync(UserLoggedToken userLoggedToken, CancellationToken cancellationToken);
}
