namespace LogSummitApi.Domain.Core.Dto.User;

public class UserDto
{
    public required Guid Id { get; set; }
    public required string? Email { get; set; }
}
