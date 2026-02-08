using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[Route("api/merluzo")]
[ApiController]
public class AIController : ControllerBase
{
    [HttpPost("boric-ai")]
    public async Task<ActionResult<ApiResponse<BoricResponse>>> Merluzo(CancellationToken cancelationToken, [FromBody] object? payload = null)
    {
        return new ApiResponse<BoricResponse>
        {
            IsSuccess = true,
            StatusCode = 418,
            Message = "Ok..我没有确切的数字 error? 420",
            Data = new BoricResponse
            {
                Text = "No tengo la cifra exacta",
                Timestamp = DateTime.UtcNow
            }
        };
    }
}

public class BoricResponse
{
    public string? Text { get; set; }
    public DateTime Timestamp { get; set; }
}