using Bogus;
using LogSummitApi.Domain.Core.Dto.User;
using LogSummitApi.Domain.Core.Entities;

namespace LogSummitApi.Tests.Helpers.Extensions;

public static class UserTestExtensions
{
    private static readonly Faker<User> _userFaker = new Faker<User>()
        .RuleFor(u => u.Id, f => f.Random.Guid())
        .RuleFor(u => u.Username, f => f.Lorem.Word())
        .RuleFor(u => u.Email, f => f.Internet.Email())
        .RuleFor(u => u.Password, f => f.Internet.Password());

    public static User WithFakeData(this User user)
    {
        return _userFaker.Generate();
    }

    public static RegisterUserDto ToRegisterUserDto(this User user)
    {
        return new RegisterUserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Password = user.Password,
            ConfirmPassword = user.Password,
        };
    }
}
