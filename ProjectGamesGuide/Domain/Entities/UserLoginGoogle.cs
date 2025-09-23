using System.ComponentModel.DataAnnotations;

namespace ProjectGamesGuide.Domain.Entities;

public class UserLoginGoogle
{
    [EmailAddress]
    [MaxLength(100)]
    public required string Email { get; set; }
    public required string GoogleSUB { get; set; }
    public required string GoogleJTI { get; set; }
}
