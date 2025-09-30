namespace MyMauiApp.Models;

public class AuthUserLogged
{
    public Guid User_Id { get; set; }
    public Guid SqlToken { get; set; }
    public required string Role { get; set; }
    public required string ExpireMin { get; set; }
    public required string ApiToken { get; set; }
}
