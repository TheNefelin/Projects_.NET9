namespace ProjectAuth.Infrastructure.Models;

public class JwtConfig
{
    public required string Key { get; set; }
    public required string Issuer { get; set; }
    public required string Audience { get; set; }
    public required string Subject { get; set; }
    public required string ExpireMin { get; set; }
}
