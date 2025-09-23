namespace ProjectGamesGuide.Domain.Entities;

public class Source
{
    public int Source_Id { get; set; }
    public string? Name { get; set; }
    public string? Url { get; set; }
    public int Game_Id { get; set; }
}
