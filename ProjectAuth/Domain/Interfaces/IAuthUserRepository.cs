using Infrastructure.Models;
using ProjectAuth.Domain.Entities;

namespace ProjectAuth.Domain.Interfaces;

public interface IAuthUserRepository
{
    Task<SqlResponse?> CreateUserAsync(AuthUser authUser, CancellationToken cancellationToken);
    Task<AuthUser?> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
    Task<Guid> NewSqlToken(string email, CancellationToken cancellationToken);
}
