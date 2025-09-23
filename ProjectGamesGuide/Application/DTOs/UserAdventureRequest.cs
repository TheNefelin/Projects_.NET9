using ProjectGamesGuide.Domain.Entities;

namespace ProjectGamesGuide.Application.DTOs;

public class UserAdventureRequest
{
    public int Adventure_Id { get; set; }
    public bool IsCheck { get; set; }
    public required UserLoggedToken UserToken { get; set; }
}
