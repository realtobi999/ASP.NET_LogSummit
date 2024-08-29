using GeoCoordinates.Core;
using LogSummitApi.Application.Core.Validators;
using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Exceptions.Http;
using LogSummitApi.Domain.Core.Interfaces.Repositories;
using LogSummitApi.Tests.Helpers.Extensions;
using Moq;

namespace LogSummitApi.Tests.Unit.Validators;

public class RouteValidatorTests
{
    private readonly Mock<IRepositoryManager> _mock;
    private readonly RouteValidator _validator;

    public RouteValidatorTests()
    {
        _mock = new Mock<IRepositoryManager>();
        _validator = new RouteValidator(_mock.Object);
    }

    [Fact]
    public async void IsValidAsync_ReturnsFalseWhenUserDoesntExist()
    {
        // prepare
        var user = new User().WithFakeData(); 
        var summit = new Summit().WithFakeData(user);
        var route = new Route().WithFakeData(user, summit);

        _mock.Setup(r => r.Users.GetAsync(route.UserId)).ReturnsAsync((User?) null); // return null => entity not found

        // act & assert
        var (isValid , exception) = await _validator.IsValidAsync(route);

        isValid.Should().BeFalse();
        exception.Should().BeOfType<NotFound404Exception>();
        exception!.Message.Should().Be($"The requested {nameof(User)} with the key '{route.UserId}' was not found in the system.");
    }

    [Fact]
    public async void IsValidAsync_ReturnsFalseWhenSummitDoesntExist()
    {
        // prepare
        var user = new User().WithFakeData(); 
        var summit = new Summit().WithFakeData(user);
        var route = new Route().WithFakeData(user, summit);

        _mock.Setup(r => r.Users.GetAsync(route.UserId)).ReturnsAsync(user);
        _mock.Setup(r => r.Summit.GetAsync(route.SummitId)).ReturnsAsync((Summit?) null); // return null => entity not found

        // act & assert
        var (isValid , exception) = await _validator.IsValidAsync(route);

        isValid.Should().BeFalse();
        exception.Should().BeOfType<NotFound404Exception>();
        exception!.Message.Should().Be($"The requested {nameof(Summit)} with the key '{route.SummitId}' was not found in the system.");
    }

    [Fact]
    public async void IsValidAsync_ReturnsFalseWhenSummitIsPrivateAndRouteUserIsNotTheOwner()
    {
        // prepare
        var user1 = new User().WithFakeData(); 
        var user2 = new User().WithFakeData();
        var summit = new Summit().WithFakeData(user1);
        var route = new Route().WithFakeData(user2, summit); // set the user of the route to a different user then the owner

        summit.IsPublic = false;

        _mock.Setup(r => r.Users.GetAsync(route.UserId)).ReturnsAsync(user2);
        _mock.Setup(r => r.Summit.GetAsync(route.SummitId)).ReturnsAsync(summit);

        // act & assert
        var (isValid , exception) = await _validator.IsValidAsync(route);

        isValid.Should().BeFalse();
        exception.Should().BeOfType<NotAuthorized401Exception>();
    } 

    [Fact]
    public async Task IsValidAsync_ReturnsFalseWhenTheSummitIsPrivateAndRoutePublicAsync()
    {
        // prepare
        var user = new User().WithFakeData();
        var summit = new Summit().WithFakeData(user);
        var route = new Route().WithFakeData(user, summit); 

        summit.IsPublic = false;

        _mock.Setup(r => r.Users.GetAsync(route.UserId)).ReturnsAsync(user);
        _mock.Setup(r => r.Summit.GetAsync(route.SummitId)).ReturnsAsync(summit);
       
        // act & assert
        var (isValid , exception) = await _validator.IsValidAsync(route);

        isValid.Should().BeFalse();
        exception.Should().BeOfType<BadRequest400Exception>();
        exception!.Message.Should().Be($"Cannot set the route to public because the summit is private. Please set the summit to public or the route to private.");
    }  

    [Fact]
    public async Task IsValidAsync_ReturnsFalseWhenTheRouteLastCoordinateIsNotCloseToTheSummitLastCoordinate()
    {
        // prepare
        var user = new User().WithFakeData();
        var summit = new Summit().WithFakeData(user);
        var route = new Route().WithFakeData(user, summit); 

        // set the last coordinate to a completely different coordinate by reversing the sign of the latitude and longitude
        route.Coordinates[^1] = new Coordinate(-summit.Coordinate!.Latitude, -summit.Coordinate.Longitude, 0);

        _mock.Setup(r => r.Users.GetAsync(route.UserId)).ReturnsAsync(user);
        _mock.Setup(r => r.Summit.GetAsync(route.SummitId)).ReturnsAsync(summit);
       
        // act & assert
        var (isValid , exception) = await _validator.IsValidAsync(route);

        isValid.Should().BeFalse();
        exception.Should().BeOfType<BadRequest400Exception>();
        exception!.Message.Should().Be($"The last coordinate of the summit route is not within a {Summit.FINAL_COORDINATE_TOLERANCE_RADIUS}-meter range of the summit coordinate.");
    }  
}
