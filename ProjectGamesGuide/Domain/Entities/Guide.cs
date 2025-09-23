namespace ProjectGamesGuide.Domain.Entities;

public class Guide
{
    public int Guide_Id { get; set; }
    public string? Name { get; set; }
    public int Sort { get; set; }
    public int Game_Id { get; set; }
}
