namespace ProjectGamesGuide.Domain.Entities;

public class BackgroundImg
{
    public int BackgroundImg_Id { get; set; }
    public string ImgUrl { get; set; } = string.Empty;
    public int Game_Id { get; set; }
}
