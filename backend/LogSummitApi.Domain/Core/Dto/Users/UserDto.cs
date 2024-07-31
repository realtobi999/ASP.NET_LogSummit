namespace LogSummitApi.Domain.Core.Dto.Users;

public class UserDto
{
    public Guid Id { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}
