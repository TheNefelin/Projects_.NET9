using ProjectPortfolio.Domain.Entities;

namespace ProjectPortfolio.Application.DTOs;

public class UrlResponse
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public bool IsEnable { get; set; }
    public ICollection<Url> Urls { get; set; } = new List<Url>();
}
