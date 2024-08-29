using System.ComponentModel.DataAnnotations;
using LogSummitApi.Domain.Core.Attributes.Validation;

namespace LogSummitApi.Domain.Core.Dto.Users;

public record class CreateUserDto
{
    public Guid? Id { get; init; }

    [Required, MaxLength(155)]
    public string? Username { get; init; }

    [Required, EmailAddress, MaxLength(155)]
    public string? Email { get; init; }

    [Required, MinLength(8), MaxLength(155)]
    public string? Password { get; init; }

    [Required, MinLength(8), MaxLength(155), SameAs(nameof(Password))]
    public string? ConfirmPassword { get; init; }
}
