using System.ComponentModel.DataAnnotations;

namespace LogSummitApi.Domain.Core.Dto.Users;

public record class LoginUserDto
{
    [Required, EmailAddress, MaxLength(155)]
    public string? Email { get; init; }

    [Required, MinLength(8), MaxLength(155)]
    public string? Password { get; init; }
}
