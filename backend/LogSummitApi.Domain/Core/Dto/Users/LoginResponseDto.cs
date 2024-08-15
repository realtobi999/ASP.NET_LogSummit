namespace LogSummitApi.Domain.Core.Dto.Users;

public record class LoginResponseDto
{
    public UserDto? User { get; set; }
    public string? Token { get; set; }
}
