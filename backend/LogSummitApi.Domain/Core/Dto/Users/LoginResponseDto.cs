namespace LogSummitApi.Domain.Core.Dto.Users;

public record class LoginResponseDto
{
    public string? Token { get; set; }
}
