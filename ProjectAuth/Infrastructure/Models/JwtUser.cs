namespace ProjectAuth.Infrastructure.Models;

public class JwtUser
{
    public Guid User_Id { get; set; }
    public required string Email { get; set; }
    public required string Role { get; set; }
}
