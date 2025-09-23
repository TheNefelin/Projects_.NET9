namespace ProjectGamesGuide.Domain.Entities;

public class Character
{
    public int Character_Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ImgUrl { get; set; }
    public int Game_Id { get; set; }
}
