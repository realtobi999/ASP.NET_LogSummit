using System.ComponentModel.DataAnnotations;
using LogSummitApi.Domain.Core.Attributes.Validation;

namespace LogSummitApi.Domain.Core.Dto.Users;

public record class CreateUserDto
{
    public Guid? Id { get; set; }

    [Required, MaxLength(155)]
    public string? Username { get; set; }

    [Required, EmailAddress, MaxLength(155)]
    public string? Email { get; set; }

    [Required, MinLength(8), MaxLength(155)]
    public string? Password { get; set; }

    [Required, MinLength(8), MaxLength(155), SameAs(nameof(Password))]
    public string? ConfirmPassword { get; set; }
}
