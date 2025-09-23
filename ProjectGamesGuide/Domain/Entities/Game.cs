namespace ProjectGamesGuide.Domain.Entities;

public class Game
{
    public int Game_Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ImgUrl { get; set; }
    public bool IsEnabled { get; set; }
}
