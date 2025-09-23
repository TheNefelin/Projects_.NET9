namespace ProjectPortfolio.Domain.Entities;

public class Url
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Link { get; set; }
    public bool IsEnable { get; set; }
    public int Id_UrlGrp { get; set; }
}
