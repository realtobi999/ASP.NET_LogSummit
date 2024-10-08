﻿using Bogus;
using LogSummitApi.Domain.Core.Dto.Users;
using LogSummitApi.Domain.Core.Entities;

namespace LogSummitApi.Tests.Helpers.Extensions;

public static class UserTestExtensions
{
    private static readonly Faker<User> _userFaker = new Faker<User>()
        .RuleFor(u => u.Id, f => f.Random.Guid())
        .RuleFor(u => u.Username, f => f.Lorem.Word())
        .RuleFor(u => u.Email, f => f.Internet.Email())
        .RuleFor(u => u.Password, f => f.Internet.Password());

    public static User WithFakeData(this User _)
    {
        return _userFaker.Generate();
    }

    public static CreateUserDto ToCreateUserDto(this User user)
    {
        return new CreateUserDto
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Password = user.Password,
            ConfirmPassword = user.Password,
        };
    }
}
