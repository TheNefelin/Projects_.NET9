namespace ProjectGamesGuide.Domain.Entities;

public class Adventure
{
    public int Adventure_Id { get; set; }
    public string? Description { get; set; }
    public bool IsImportant { get; set; }
    public int Sort { get; set; }
    public int Guide_Id { get; set; }
}
