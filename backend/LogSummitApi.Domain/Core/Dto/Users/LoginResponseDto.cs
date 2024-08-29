namespace LogSummitApi.Domain.Core.Dto.Users;

public record class LoginResponseDto
{
    public UserDto? User { get; init; }
    public string? Token { get; init; }
}
