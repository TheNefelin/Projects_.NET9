using WebApiPM.Domain.Entities;

namespace WebApiPM.Application.Interfaces;

public record TokenResult(string Token, int ExpireMin);

public interface IAuthTokenService
{
    TokenResult GenerateToken(AuthUser user);
}