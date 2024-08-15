namespace LogSummitApi.Domain.Core.Dto.Users;

public class UserDto
{
    public required Guid Id { get; set; }
    public required string? Email { get; set; }
}
