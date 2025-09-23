using ProjectGamesGuide.Domain.Entities;

namespace ProjectGamesGuide.Application.DTOs;

public class AdventureResponse
{
    public int Adventure_Id { get; set; }
    public string? Description { get; set; }
    public bool IsImportant { get; set; }
    public int Sort { get; set; }
    public UserAdventure AdventureUser { get; set; }
    public ICollection<AdventureImgResponse> AdventureImg { get; set; }
}
