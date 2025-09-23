namespace ProjectGamesGuide.Domain.Entities;

public class AdventureImg
{
    public int AdventureImg_Id { get; set; }
    public string? ImgUrl { get; set; }
    public int Sort { get; set; }
    public int Adventure_Id { get; set; }
}
