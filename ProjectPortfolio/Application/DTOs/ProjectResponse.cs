using ProjectPortfolio.Domain.Entities;

namespace ProjectPortfolio.Application.DTOs;

public class ProjectResponse
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? ImgUrl { get; set; }
    public string? RepoUrl { get; set; }
    public string? AppUrl { get; set; }
    public bool IsEnabled { get; set; }
    public ICollection<Language> Languages { get; set; } = new List<Language>();
    public ICollection<Technology> Technologies { get; set; } = new List<Technology>();
}
