using System.ComponentModel.DataAnnotations;

namespace ProjectAuth.Application.DTOs;

public class AuthUserLogin
{
    [EmailAddress]
    [MaxLength(100)]
    public required string Email { get; set; } = string.Empty;

    [MinLength(6)]
    [MaxLength(50)]
    public required string Password { get; set; } = string.Empty;
}
