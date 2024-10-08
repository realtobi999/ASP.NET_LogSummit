using System.Security.Claims;
using GeoCoordinates.Core;
using LogSummitApi.Domain.Core.Dto.Summits.Routes;
using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Exceptions.Http;
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

        var create1 = await client.PostAsJsonAsync("v1/api/auth/register", user.ToCreateUserDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("v1/api/summit", summit.ToCreateSummitDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var response = await client.PostAsJsonAsync("v1/api/route", route.ToCreateRouteDto());
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var header = response.Headers.GetValues("Location");
        header.Should().Equal(string.Format("/v1/api/route/{0}", route.Id));
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

        var create1 = await client.PostAsJsonAsync("v1/api/auth/register", user.ToCreateUserDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("v1/api/summit", summit.ToCreateSummitDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var response = await client.PostAsJsonAsync("v1/api/route", route.ToCreateRouteDto());
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ErrorMessage>() ?? throw new NullReferenceException();

        content.Title.Should().Be("Bad Request");
        content.Status.Should().Be((int)HttpStatusCode.BadRequest);
        content.Instance.Should().Be("POST /v1/api/route");
        content.Type.Should().Be(nameof(BadRequest400Exception));
        content.Detail.Should().Contain(Summit.FINAL_COORDINATE_TOLERANCE_RADIUS.ToString());
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

        var create1 = await client.PostAsJsonAsync("v1/api/auth/register", user.ToCreateUserDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("v1/api/summit", summit.ToCreateSummitDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);

        var create3 = await client.PostAsJsonAsync("v1/api/route", route1.ToCreateRouteDto());
        create3.StatusCode.Should().Be(HttpStatusCode.Created);
        var create4 = await client.PostAsJsonAsync("v1/api/route", route2.ToCreateRouteDto());
        create4.StatusCode.Should().Be(HttpStatusCode.Created);
        var create5 = await client.PostAsJsonAsync("v1/api/route", route3.ToCreateRouteDto());
        create5.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var offset = 1;
        var limit = 2;

        var response = await client.GetAsync($"v1/api/route?limit={limit}&offset={offset}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<RouteDto>>() ?? throw new NullReferenceException();

        content.Count.Should().Be(limit);
        content.ElementAt(0).Id.Should().Be(route2.Id);
    }

    [Fact]
    public async void Index_Returns200AndUserFilterWorks()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user1 = new User().WithFakeData();
        var user2 = new User().WithFakeData();
        var summit = new Summit().WithFakeData(user1);
        var route1 = new Route().WithFakeData(user1, summit);
        var route2 = new Route().WithFakeData(user1, summit);
        var route3 = new Route().WithFakeData(user2, summit);

        var jwt1 = JwtTestUtils.CreateInstance().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user1.Id.ToString()),
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"BEARER {jwt1}");

        var create1 = await client.PostAsJsonAsync("v1/api/auth/register", user1.ToCreateUserDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create6 = await client.PostAsJsonAsync("v1/api/auth/register", user2.ToCreateUserDto());
        create6.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("v1/api/summit", summit.ToCreateSummitDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);

        var create3 = await client.PostAsJsonAsync("v1/api/route", route1.ToCreateRouteDto());
        create3.StatusCode.Should().Be(HttpStatusCode.Created);
        var create4 = await client.PostAsJsonAsync("v1/api/route", route2.ToCreateRouteDto());
        create4.StatusCode.Should().Be(HttpStatusCode.Created);

        var jwt2 = JwtTestUtils.CreateInstance().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user2.Id.ToString()),
        ]);
        client.DefaultRequestHeaders.Remove("Authorization");
        client.DefaultRequestHeaders.Add("Authorization", $"BEARER {jwt2}");

        var create5 = await client.PostAsJsonAsync("v1/api/route", route3.ToCreateRouteDto());
        create5.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var response = await client.GetAsync($"v1/api/route?userId={user2.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<RouteDto>>() ?? throw new NullReferenceException();

        content.Count.Should().Be(1);
        content.ElementAt(0).Id.Should().Be(route3.Id);
    }

    [Fact]
    public async void Index_Returns200AndSummitFilterWorks()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();
        var summit1 = new Summit().WithFakeData(user);
        var summit2 = new Summit().WithFakeData(user);
        var route1 = new Route().WithFakeData(user, summit1);
        var route2 = new Route().WithFakeData(user, summit1);
        var route3 = new Route().WithFakeData(user, summit2);

        var jwt = JwtTestUtils.CreateInstance().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString()),
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"BEARER {jwt}");

        var create1 = await client.PostAsJsonAsync("v1/api/auth/register", user.ToCreateUserDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("v1/api/summit", summit1.ToCreateSummitDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);
        var create6 = await client.PostAsJsonAsync("v1/api/summit", summit2.ToCreateSummitDto());
        create6.StatusCode.Should().Be(HttpStatusCode.Created);

        var create3 = await client.PostAsJsonAsync("v1/api/route", route1.ToCreateRouteDto());
        create3.StatusCode.Should().Be(HttpStatusCode.Created);
        var create4 = await client.PostAsJsonAsync("v1/api/route", route2.ToCreateRouteDto());
        create4.StatusCode.Should().Be(HttpStatusCode.Created);
        var create5 = await client.PostAsJsonAsync("v1/api/route", route3.ToCreateRouteDto());
        create5.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var response = await client.GetAsync($"v1/api/route?summitId={summit2.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<RouteDto>>() ?? throw new NullReferenceException();

        content.Count.Should().Be(1);
        content.ElementAt(0).Id.Should().Be(route3.Id);
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

        var create1 = await client.PostAsJsonAsync("v1/api/auth/register", user.ToCreateUserDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("v1/api/summit", summit.ToCreateSummitDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("v1/api/route", route.ToCreateRouteDto());
        create3.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var response1 = await client.GetAsync($"v1/api/route/{Guid.NewGuid()}");
        response1.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var response2 = await client.GetAsync($"v1/api/route/{route.Id}");
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

        var create1 = await client.PostAsJsonAsync("v1/api/auth/register", user.ToCreateUserDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("v1/api/summit", summit.ToCreateSummitDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("v1/api/route", route.ToCreateRouteDto());
        create3.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var updateDto = new UpdateRouteDto
        {

            Name = "test",
            Description = "test_test_test_test_test",
            IsPublic = route.IsPublic,
            Coordinates = route.Coordinates,
        };

        var response = await client.PutAsJsonAsync($"v1/api/route/{route.Id}", updateDto);
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var get = await client.GetAsync($"v1/api/route/{route.Id}");
        get.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await get.Content.ReadFromJsonAsync<RouteDto>() ?? throw new NullReferenceException();

        content.Id.Should().Be(route.Id);
        content.Name.Should().Be(updateDto.Name);
        content.Description.Should().Be(updateDto.Description);
    }

    [Fact]
    public async void Delete_Returns204AndIsDeleted()
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

        var create1 = await client.PostAsJsonAsync("v1/api/auth/register", user.ToCreateUserDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("v1/api/summit", summit.ToCreateSummitDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("v1/api/route", route.ToCreateRouteDto());
        create3.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var response = await client.DeleteAsync($"v1/api/route/{route.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var get = await client.GetAsync($"v1/api/route/{route.Id}");
        get.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async void Create_Returns400WhenTryingToCreateAPublicRouteOnAPrivateSummit()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();
        var summit = new Summit().WithFakeData(user);
        var route = new Route().WithFakeData(user, summit);

        summit.IsPublic = false;

        var jwt = JwtTestUtils.CreateInstance().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString()),
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"BEARER {jwt}");

        var create1 = await client.PostAsJsonAsync("v1/api/auth/register", user.ToCreateUserDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("v1/api/summit", summit.ToCreateSummitDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var response = await client.PostAsJsonAsync("v1/api/route", route.ToCreateRouteDto());
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ErrorMessage>() ?? throw new NullReferenceException();

        content.Detail.Should().Contain("Cannot set the route to public");
    }
}
