namespace ProjectGamesGuide.Domain.Entities;

public class UserAdventure
{
    public Guid User_Id { get; set; }
    public int Adventure_Id { get; set; }
    public bool IsCheck { get; set; }
}
