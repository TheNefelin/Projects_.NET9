namespace ProjectGamesGuide.Application.DTOs;

public class GameResponse
{
    public int Game_Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? ImgUrl { get; set; }
    public bool IsEnabled { get; set; }
    public ICollection<CharacterResponse> Characters { get; set; }
    public ICollection<SourceResponse> Sources { get; set; }
    public ICollection<BackgroundImgResponse> BackgroundImgs { get; set; }
    public ICollection<GuideResponse> guides { get; set; }
}
