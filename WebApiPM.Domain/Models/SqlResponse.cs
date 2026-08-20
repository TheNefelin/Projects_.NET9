namespace WebApiPM.Domain.Models;

public class SqlResponse
{
    public bool IsSuccess { get; set; }
    public int StatusCode { get; set; }
    public string? Message { get; set; }
}