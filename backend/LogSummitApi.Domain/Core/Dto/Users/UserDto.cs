namespace LogSummitApi.Domain.Core.Dto.Users;

public class UserDto
{
    public required Guid Id { get; init; }
    public required string? Email { get; init; }
}
