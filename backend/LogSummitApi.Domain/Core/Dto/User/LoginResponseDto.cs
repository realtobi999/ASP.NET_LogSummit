namespace LogSummitApi.Domain.Core.Dto.User;

public record class LoginResponseDto
{
    public string? Token { get; set; }
}
