using System.Security.Claims;
using LogSummitApi.Domain.Core.Dto.Summit.Routes;
using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Exceptions.Http;
using LogSummitApi.Domain.Core.Utilities.Coordinates;
using LogSummitApi.Presentation;
using LogSummitApi.Tests.Helpers;
using LogSummitApi.Tests.Helpers.Extensions;
using LogSummitApi.Tests.Integration.Server;

namespace LogSummitApi.Tests.Integration.Endpoints;

public class RouteTests
{
    [Fact]
    public async void Create_Returns200AndLocationHeader()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();
        var summit = new Summit().WithFakeData(user);
        var route = new Route().WithFakeData(user, summit);

        var jwt = JwtTestUtils.CreateInstance().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString()),
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"BEARER {jwt}");

        var create1 = await client.PostAsJsonAsync("v1/api/auth/register", user.ToRegisterUserDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("v1/api/summit", summit.ToCreateSummitDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var response = await client.PostAsJsonAsync("v1/api/summit/route", route.ToCreateRouteDto());
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var header = response.Headers.GetValues("Location");
        header.Should().Equal(string.Format("/v1/api/summit/route/{0}", route.Id));
    }

    [Fact]
    public async void Create_Returns400BecauseTheLastCoordinateIsOutsideOfTheSummitRange()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();
        var summit = new Summit().WithFakeData(user);
        var route = new Route().WithFakeData(user, summit);

        // here we set the last summit route coordinate something entirely different than the summit coordinate
        route.Coordinates[^1] = new Coordinate(summit.Coordinate!.Latitude * 0.5, summit.Coordinate!.Longitude * 0.5, 100);

        var jwt = JwtTestUtils.CreateInstance().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString()),
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"BEARER {jwt}");

        var create1 = await client.PostAsJsonAsync("v1/api/auth/register", user.ToRegisterUserDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("v1/api/summit", summit.ToCreateSummitDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var response = await client.PostAsJsonAsync("v1/api/summit/route", route.ToCreateRouteDto());
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ErrorMessage>() ?? throw new NullReferenceException();

        content.Title.Should().Be("Bad Request");
        content.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        content.Instance.Should().Be("POST /v1/api/summit/route");
        content.Type.Should().Be(nameof(BadRequest400Exception));
        content.Detail.Should().Contain(Summit.RouteRadius.ToString());
    }

    [Fact]
    public async void Index_Returns200AndLimitAndOffsetWorks()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();
        var summit = new Summit().WithFakeData(user);
        var route1 = new Route().WithFakeData(user, summit);
        var route2 = new Route().WithFakeData(user, summit);
        var route3 = new Route().WithFakeData(user, summit);

        var jwt = JwtTestUtils.CreateInstance().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString()),
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"BEARER {jwt}");

        var create1 = await client.PostAsJsonAsync("v1/api/auth/register", user.ToRegisterUserDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("v1/api/summit", summit.ToCreateSummitDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);

        var create3 = await client.PostAsJsonAsync("v1/api/summit/route", route1.ToCreateRouteDto());
        create3.StatusCode.Should().Be(HttpStatusCode.Created);
        var create4 = await client.PostAsJsonAsync("v1/api/summit/route", route2.ToCreateRouteDto());
        create4.StatusCode.Should().Be(HttpStatusCode.Created);
        var create5 = await client.PostAsJsonAsync("v1/api/summit/route", route3.ToCreateRouteDto());
        create5.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var offset = 1;
        var limit = 2;

        var response = await client.GetAsync($"v1/api/summit/route?limit={limit}&offset={offset}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<RouteDto>>() ?? throw new NullReferenceException();

        content.Count.Should().Be(limit);
        content.ElementAt(0).Id.Should().Be(route2.Id);
    }

    [Fact]
    public async void Get_Returns200And404()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();
        var summit = new Summit().WithFakeData(user);
        var route = new Route().WithFakeData(user, summit);

        var jwt = JwtTestUtils.CreateInstance().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString()),
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"BEARER {jwt}");

        var create1 = await client.PostAsJsonAsync("v1/api/auth/register", user.ToRegisterUserDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("v1/api/summit", summit.ToCreateSummitDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("v1/api/summit/route", route.ToCreateRouteDto());
        create3.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var response1 = await client.GetAsync($"v1/api/summit/route/{Guid.NewGuid()}");
        response1.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var response2 = await client.GetAsync($"v1/api/summit/route/{route.Id}");
        response2.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response2.Content.ReadFromJsonAsync<RouteDto>() ?? throw new NullReferenceException();

        content.Id.Should().Be(route.Id);
    }

    [Fact]
    public async void Update_Returns204AndIsUpdated()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();
        var summit = new Summit().WithFakeData(user);
        var route = new Route().WithFakeData(user, summit);

        var jwt = JwtTestUtils.CreateInstance().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString()),
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"BEARER {jwt}");

        var create1 = await client.PostAsJsonAsync("v1/api/auth/register", user.ToRegisterUserDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("v1/api/summit", summit.ToCreateSummitDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("v1/api/summit/route", route.ToCreateRouteDto());
        create3.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var updateDto = new UpdateRouteDto
        {
            UserId = route.UserId,
            SummitId = route.SummitId,
            Name = "test",
            Description = "test",
            Distance = route.Distance,
            ElevationGain = route.ElevationGain,
            ElevationLoss = route.ElevationLoss,
            Coordinates = route.Coordinates,
        };

        var response = await client.PutAsJsonAsync($"v1/api/summit/route/{route.Id}", updateDto);
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var get = await client.GetAsync($"v1/api/summit/route/{route.Id}");
        get.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await get.Content.ReadFromJsonAsync<RouteDto>() ?? throw new NullReferenceException();

        content.Id.Should().Be(route.Id);
        content.Name.Should().Be(updateDto.Name);
        content.Description.Should().Be(updateDto.Description);
    }
}
