using ProjectGamesGuide.Domain.Entities;

namespace ProjectGamesGuide.Application.DTOs;

public class GuideResponse
{
    public int Guide_Id { get; set; }
    public string? Name { get; set; }
    public int Sort { get; set; }
    public UserGuide GuideUser { get; set; }
    public ICollection<AdventureResponse> Adventures { get; set; }
}
