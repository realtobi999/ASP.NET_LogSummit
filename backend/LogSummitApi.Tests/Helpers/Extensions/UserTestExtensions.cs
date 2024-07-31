using Bogus;
using LogSummitApi.Domain.Core.Dto.Users;
using LogSummitApi.Domain.Core.Entities;

namespace LogSummitApi.Tests.Helpers.Extensions;

public static class UserTestExtensions
{
    private static readonly Faker<User> _userFaker = new Faker<User>()
        .RuleFor(u => u.Id, f => f.Random.Guid())
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
            Email = user.Email,
            Password = user.Password,
            ConfirmPassword = user.Password,
        };
    }
}
