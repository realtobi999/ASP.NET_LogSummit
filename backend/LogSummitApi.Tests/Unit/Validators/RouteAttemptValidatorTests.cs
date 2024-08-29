using System;
using GeoCoordinates.Core;
using LogSummitApi.Application.Core.Validators;
using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Exceptions.Http;
using LogSummitApi.Domain.Core.Interfaces.Repositories;
using LogSummitApi.Tests.Helpers.Extensions;
using Moq;

namespace LogSummitApi.Tests.Unit.Validators;

public class RouteAttemptValidatorTests
{
    private readonly Mock<IRepositoryManager> _mock;
    private readonly RouteAttemptValidator _validator;

    public RouteAttemptValidatorTests()
    {
        _mock = new Mock<IRepositoryManager>();
        _validator = new RouteAttemptValidator(_mock.Object);
    }

    [Fact]
    public async void IsValidAsync_ReturnsFalseWhenUserDoesntExist()
    {
        // prepare
        var user = new User().WithFakeData();
        var summit = new Summit().WithFakeData(user);
        var route = new Route().WithFakeData(user, summit);
        var attempt = new RouteAttempt().WithFakeData(user, route);

        _mock.Setup(r => r.Users.GetAsync(attempt.UserId)).ReturnsAsync((User?) null); // set to return null => entity not found

        // act & assert
        var (isValid, exception) = await _validator.IsValidAsync(attempt);

        isValid.Should().BeFalse();
        exception.Should().BeOfType<NotFound404Exception>();
        exception!.Message.Should().Be($"The requested {nameof(User)} with the key '{attempt.UserId}' was not found in the system.");
    }

    [Fact]
    public async void IsValidAsync_ReturnsFalseWhenSummitDoesntExist()
    {
        // prepare
        var user = new User().WithFakeData();
        var summit = new Summit().WithFakeData(user);
        var route = new Route().WithFakeData(user, summit);
        var attempt = new RouteAttempt().WithFakeData(user, route);

        _mock.Setup(r => r.Users.GetAsync(attempt.UserId)).ReturnsAsync(user);
        _mock.Setup(r => r.Summit.GetAsync(attempt.SummitId)).ReturnsAsync((Summit?) null); // set to return null => entity not found

        // act & assert
        var (isValid, exception) = await _validator.IsValidAsync(attempt);

        isValid.Should().BeFalse();
        exception.Should().BeOfType<NotFound404Exception>();
        exception!.Message.Should().Be($"The requested {nameof(Summit)} with the key '{attempt.SummitId}' was not found in the system.");
    }

    [Fact]
    public async void IsValidAsync_ReturnsFalseWhenRouteDoesntExist()
    {
        // prepare
        var user = new User().WithFakeData();
        var summit = new Summit().WithFakeData(user);
        var route = new Route().WithFakeData(user, summit);
        var attempt = new RouteAttempt().WithFakeData(user, route);

        _mock.Setup(r => r.Users.GetAsync(attempt.UserId)).ReturnsAsync(user);
        _mock.Setup(r => r.Summit.GetAsync(attempt.SummitId)).ReturnsAsync(summit);
        _mock.Setup(r => r.Route.GetAsync(attempt.RouteId)).ReturnsAsync((Route?) null); // set to return ull => entity not found

        // act & assert
        var (isValid, exception) = await _validator.IsValidAsync(attempt);

        isValid.Should().BeFalse();
        exception.Should().BeOfType<NotFound404Exception>();
        exception!.Message.Should().Be($"The requested {nameof(Route)} with the key '{attempt.RouteId}' was not found in the system.");
    }

    [Fact]
    public async void IsValidAsync_ReturnsFalseWhenRouteIsPrivateAndAttemptUserIsNotTheOwner()
    {
        // prepare
        var user1 = new User().WithFakeData();
        var user2 = new User().WithFakeData();
        var summit = new Summit().WithFakeData(user1);
        var route = new Route().WithFakeData(user1, summit);
        var attempt = new RouteAttempt().WithFakeData(user2, route); // set the user of the attempt to a different user than the owner of the route

        route.IsPublic = false;

        _mock.Setup(r => r.Users.GetAsync(attempt.UserId)).ReturnsAsync(user2);
        _mock.Setup(r => r.Summit.GetAsync(attempt.SummitId)).ReturnsAsync(summit);
        _mock.Setup(r => r.Route.GetAsync(attempt.RouteId)).ReturnsAsync(route);

        // act & assert
        var (isValid, exception) = await _validator.IsValidAsync(attempt);

        isValid.Should().BeFalse();
        exception.Should().BeOfType<NotAuthorized401Exception>();
    }

    [Fact]
    public async void IsValidAsync_ReturnsFalseWhenRouteIsPrivateAndAttemptPublic()
    {
        // prepare
        var user = new User().WithFakeData();
        var summit = new Summit().WithFakeData(user);
        var route = new Route().WithFakeData(user, summit);
        var attempt = new RouteAttempt().WithFakeData(user, route);

        route.IsPublic = false;

        _mock.Setup(r => r.Users.GetAsync(attempt.UserId)).ReturnsAsync(user);
        _mock.Setup(r => r.Summit.GetAsync(attempt.SummitId)).ReturnsAsync(summit);
        _mock.Setup(r => r.Route.GetAsync(attempt.RouteId)).ReturnsAsync(route); 

        // act & assert
        var (isValid, exception) = await _validator.IsValidAsync(attempt);

        isValid.Should().BeFalse();
        exception.Should().BeOfType<BadRequest400Exception>();
        exception!.Message.Should().Be("Cannot set the route attempt to public because the route is private. Please set the summit to public or the route to private.");
    }

    [Fact]
    public async void IsValidAsync_ReturnsFalseWhenAttemptFirstCoordinateIsNotCloseToRouteFirstCoordinate()
    {
        // prepare
        var user = new User().WithFakeData();
        var summit = new Summit().WithFakeData(user);
        var route = new Route().WithFakeData(user, summit);
        var attempt = new RouteAttempt().WithFakeData(user, route);

        // set the first coordinate of the attempt to a different coordinate than the first coordinate of the route by reversing sign of latitude and longitude
        attempt.Coordinates[0] = new Coordinate(-route.Coordinates[0].Latitude, -route.Coordinates[0].Longitude, 0);

        _mock.Setup(r => r.Users.GetAsync(attempt.UserId)).ReturnsAsync(user);
        _mock.Setup(r => r.Summit.GetAsync(attempt.SummitId)).ReturnsAsync(summit);
        _mock.Setup(r => r.Route.GetAsync(attempt.RouteId)).ReturnsAsync(route); 

        // act & assert
        var (isValid, exception) = await _validator.IsValidAsync(attempt);

        isValid.Should().BeFalse();
        exception.Should().BeOfType<BadRequest400Exception>();
        exception!.Message.Should().Be($"The first coordinate of the route attempt is not withing a {Route.FIRST_COORDINATE_TOLERANCE_RADIUS}-meter range of the fist route coordinate");
    }

    [Fact]
    public async void IsValidAsync_ReturnsFalseWhenAttemptLastCoordinateIsNotCloseToSummitCoordinate()
    {
        // prepare
        var user = new User().WithFakeData();
        var summit = new Summit().WithFakeData(user);
        var route = new Route().WithFakeData(user, summit);
        var attempt = new RouteAttempt().WithFakeData(user, route);

        // set the last coordinate of the attempt to a different coordinate than the last coordinate of the route by reversing sign of latitude and longitude
        attempt.Coordinates[^1] = new Coordinate(-summit.Coordinate!.Latitude, -summit.Coordinate.Longitude, 0);
        
        _mock.Setup(r => r.Users.GetAsync(attempt.UserId)).ReturnsAsync(user);
        _mock.Setup(r => r.Summit.GetAsync(attempt.SummitId)).ReturnsAsync(summit);
        _mock.Setup(r => r.Route.GetAsync(attempt.RouteId)).ReturnsAsync(route); 

        // act & assert
        var (isValid, exception) = await _validator.IsValidAsync(attempt);

        isValid.Should().BeFalse();
        exception.Should().BeOfType<BadRequest400Exception>();
        exception!.Message.Should().Be($"The last coordinate of the route attempt is not within a {Summit.FINAL_COORDINATE_TOLERANCE_RADIUS}-meter range of the summit coordinate.");
    }

    [Fact]
    public async void IsValidAsync_ReturnsFalseWhenAttemptCoordinatesDontAlignWithTheRouteCoordinates()
    {
        // prepare
        var user = new User().WithFakeData();
        var summit = new Summit().WithFakeData(user);
        var route = new Route().WithFakeData(user, summit);
        var attempt = new RouteAttempt().WithFakeData(user, route);

        attempt.Coordinates = [
            new(route.Coordinates[0].Latitude, route.Coordinates[0].Longitude, 0), // the same as the route first coordinate
            new(-route.Coordinates[1].Latitude, -route.Coordinates[1].Longitude, 0), // different
            new(-route.Coordinates[1].Latitude, -route.Coordinates[1].Longitude, 0), // different
            new(-route.Coordinates[1].Latitude, -route.Coordinates[1].Longitude, 0), // different
            new(route.Coordinates[2].Latitude, route.Coordinates[2].Longitude, 0), // the same as the route last coordinate
        ];
        
        _mock.Setup(r => r.Users.GetAsync(attempt.UserId)).ReturnsAsync(user);
        _mock.Setup(r => r.Summit.GetAsync(attempt.SummitId)).ReturnsAsync(summit);
        _mock.Setup(r => r.Route.GetAsync(attempt.RouteId)).ReturnsAsync(route); 

        // act & assert
        var (isValid, exception) = await _validator.IsValidAsync(attempt);

        isValid.Should().BeFalse();
        exception.Should().BeOfType<BadRequest400Exception>();
        exception!.Message.Should().Be($"The route attempt does not align with the route coordinates within the allowed deviation of {Route.ALLOWED_DEVIATION_RADIUS} meters.");
    }
}
