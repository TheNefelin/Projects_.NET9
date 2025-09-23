namespace ProjectPortfolio.Domain.Entities;

public class Project
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? ImgUrl { get; set; }
    public string? RepoUrl { get; set; }
    public string? AppUrl { get; set; }
}
