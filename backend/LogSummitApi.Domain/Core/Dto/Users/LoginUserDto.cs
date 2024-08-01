using System.ComponentModel.DataAnnotations;

namespace LogSummitApi.Domain.Core.Dto.Users;

public class LoginUserDto
{
    [Required, EmailAddress, MaxLength(155)]
    public string? Email { get; set; }

    [Required, MinLength(8), MaxLength(155)]
    public string? Password { get; set; }
}
