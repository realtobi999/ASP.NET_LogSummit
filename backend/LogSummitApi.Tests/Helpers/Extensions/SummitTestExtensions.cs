﻿using Bogus;
using LogSummitApi.Domain.Core.Dto.Summit;
using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Utilities.Coordinates;

namespace LogSummitApi.Tests.Helpers.Extensions;

public static class SummitTestExtensions
{
    private static readonly Faker<Summit> _summitFaker = new Faker<Summit>()
        .RuleFor(s => s.Id, f => f.Random.Guid())
        .RuleFor(s => s.Name, f => f.Lorem.Sentence(3))
        .RuleFor(s => s.Description, f => f.Lorem.Paragraph())
        .RuleFor(s => s.CreatedAt, f => f.Date.Past())
        .RuleFor(s => s.Coordinate, f => new Coordinate(f.Address.Latitude(), f.Address.Longitude(), 100));

    public static Summit WithFakeData(this Summit _, User user)
    {
        var fakeSummit = _summitFaker.Generate();
        
        fakeSummit.UserId = user.Id;

        return fakeSummit;
    }

    public static CreateSummitDto ToCreateSummitDto(this Summit summit)
    {
        return new CreateSummitDto
        {
            Id = summit.Id,
            UserId = summit.UserId,
            Name = summit.Name,
            Description = summit.Description,
            CreatedAt = summit.CreatedAt,
            Coordinate = summit.Coordinate
        };
    }
}