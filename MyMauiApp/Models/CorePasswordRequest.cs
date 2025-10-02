namespace MyMauiApp.Models;

public class CorePasswordRequest
{
    public required string Password { get; set; }
    public required CoreUserRequest CoreUser { get; set; }
}
