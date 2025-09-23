namespace ProjectPasswordManager.Application.DTOs;

public class CoreUserPassword
{
    public required string Password { get; set; }
    public required CoreUserRequest CoreUser { get; set; }
}
