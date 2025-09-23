namespace Infrastructure.Models;

public class SqlGoogleResponse
{
    public bool IsSuccess { get; set; }
    public int StatusCode { get; set; }
    public string? Message { get; set; }
    public Guid User_Id { get; set; }
    public Guid SqlToken { get; set; }
}
