namespace ProjectGamesGuide.Domain.Entities;

public class UserLoggedToken
{
    public Guid User_Id { get; set; }
    public Guid SqlToken { get; set; }
}
