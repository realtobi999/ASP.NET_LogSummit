using System.Security.Claims;
using LogSummitApi.Domain.Core.Dto.Summits.Routes.Attempts;
using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Presentation;
using LogSummitApi.Tests.Helpers;
using LogSummitApi.Tests.Helpers.Extensions;
using LogSummitApi.Tests.Integration.Server;

namespace LogSummitApi.Tests.Integration.Endpoints;

public class RouteAttemptTests
{
    [Fact]
    public async void Create_Returns201AndLocationHeader()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();
        var summit = new Summit().WithFakeData(user);
        var route = new Route().WithFakeData(user, summit);
        var attempt = new RouteAttempt().WithFakeData(user, route);

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
        var response = await client.PostAsJsonAsync("v1/api/route/attempt", attempt.ToCreateRouteAttemptDto());
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var header = response.Headers.GetValues("Location");
        header.Should().Equal(string.Format("/v1/api/route/attempt/{0}", attempt.Id));
    }

    [Fact]
    public async Task Index_Returns200AndPaginationWorks()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();
        var summit = new Summit().WithFakeData(user);
        var route = new Route().WithFakeData(user, summit);
        var attempt1 = new RouteAttempt().WithFakeData(user, route);
        var attempt2 = new RouteAttempt().WithFakeData(user, route);
        var attempt3 = new RouteAttempt().WithFakeData(user, route);

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

        var create4 = await client.PostAsJsonAsync("v1/api/route/attempt", attempt1.ToCreateRouteAttemptDto());
        create4.StatusCode.Should().Be(HttpStatusCode.Created);
        var create5 = await client.PostAsJsonAsync("v1/api/route/attempt", attempt2.ToCreateRouteAttemptDto());
        create5.StatusCode.Should().Be(HttpStatusCode.Created);
        var create6 = await client.PostAsJsonAsync("v1/api/route/attempt", attempt3.ToCreateRouteAttemptDto());
        create6.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var limit = 2;
        var offset = 1;

        var response = await client.GetAsync($"v1/api/route/attempt?limit={limit}&offset={offset}"); 
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<RouteAttemptDto>>() ?? throw new NullReferenceException();

        content.Count.Should().Be(limit);
        content.ElementAt(0).Id.Should().Be(attempt2.Id);
    }

    [Fact]
    public async Task Index_Returns200AndUserFilterWorks()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user1 = new User().WithFakeData();
        var user2 = new User().WithFakeData();
        var summit = new Summit().WithFakeData(user1);
        var route = new Route().WithFakeData(user1, summit);
        var attempt1 = new RouteAttempt().WithFakeData(user1, route);
        var attempt2 = new RouteAttempt().WithFakeData(user1, route);
        var attempt3 = new RouteAttempt().WithFakeData(user2, route);

        var jwt1 = JwtTestUtils.CreateInstance().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user1.Id.ToString()),
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"BEARER {jwt1}");

        var create1 = await client.PostAsJsonAsync("v1/api/auth/register", user1.ToCreateUserDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create7 = await client.PostAsJsonAsync("v1/api/auth/register", user2.ToCreateUserDto());
        create7.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("v1/api/summit", summit.ToCreateSummitDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("v1/api/route", route.ToCreateRouteDto());
        create3.StatusCode.Should().Be(HttpStatusCode.Created);

        var create4 = await client.PostAsJsonAsync("v1/api/route/attempt", attempt1.ToCreateRouteAttemptDto());
        create4.StatusCode.Should().Be(HttpStatusCode.Created);
        var create5 = await client.PostAsJsonAsync("v1/api/route/attempt", attempt2.ToCreateRouteAttemptDto());
        create5.StatusCode.Should().Be(HttpStatusCode.Created);

        var jwt2 = JwtTestUtils.CreateInstance().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user2.Id.ToString()),
        ]);
        client.DefaultRequestHeaders.Remove("Authorization");
        client.DefaultRequestHeaders.Add("Authorization", $"BEARER {jwt2}");

        var create6 = await client.PostAsJsonAsync("v1/api/route/attempt", attempt3.ToCreateRouteAttemptDto());
        create6.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var response = await client.GetAsync($"v1/api/route/attempt?userId={user2.Id}"); 
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<RouteAttemptDto>>() ?? throw new NullReferenceException();

        content.Count.Should().Be(1);
        content.ElementAt(0).Id.Should().Be(attempt3.Id);
    }

    [Fact]
    public async Task Index_Returns200AndSummitFilterWorks()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();
        var summit1 = new Summit().WithFakeData(user);
        var summit2 = new Summit().WithFakeData(user);
        var route1 = new Route().WithFakeData(user, summit1);
        var route2 = new Route().WithFakeData(user, summit2);
        var attempt1 = new RouteAttempt().WithFakeData(user, route1);
        var attempt2 = new RouteAttempt().WithFakeData(user, route1);
        var attempt3 = new RouteAttempt().WithFakeData(user, route2);

        var jwt = JwtTestUtils.CreateInstance().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString()),
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"BEARER {jwt}");

        var create1 = await client.PostAsJsonAsync("v1/api/auth/register", user.ToCreateUserDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("v1/api/summit", summit1.ToCreateSummitDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);
        var create7 = await client.PostAsJsonAsync("v1/api/summit", summit2.ToCreateSummitDto());
        create7.StatusCode.Should().Be(HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("v1/api/route", route1.ToCreateRouteDto());
        create3.StatusCode.Should().Be(HttpStatusCode.Created);
        var create8 = await client.PostAsJsonAsync("v1/api/route", route2.ToCreateRouteDto());
        create8.StatusCode.Should().Be(HttpStatusCode.Created);


        var create4 = await client.PostAsJsonAsync("v1/api/route/attempt", attempt1.ToCreateRouteAttemptDto());
        create4.StatusCode.Should().Be(HttpStatusCode.Created);
        var create5 = await client.PostAsJsonAsync("v1/api/route/attempt", attempt2.ToCreateRouteAttemptDto());
        create5.StatusCode.Should().Be(HttpStatusCode.Created);
        var create6 = await client.PostAsJsonAsync("v1/api/route/attempt", attempt3.ToCreateRouteAttemptDto());
        create6.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var response = await client.GetAsync($"v1/api/route/attempt?summitId={summit2.Id}"); 
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<RouteAttemptDto>>() ?? throw new NullReferenceException();

        content.Count.Should().Be(1);
        content.ElementAt(0).Id.Should().Be(attempt3.Id);
    }

    [Fact]
    public async Task Index_Returns200AndRouteFilterWorks()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();
        var summit = new Summit().WithFakeData(user);
        var route1 = new Route().WithFakeData(user, summit);
        var route2 = new Route().WithFakeData(user, summit);
        var attempt1 = new RouteAttempt().WithFakeData(user, route1);
        var attempt2 = new RouteAttempt().WithFakeData(user, route1);
        var attempt3 = new RouteAttempt().WithFakeData(user, route2);

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
        var create7 = await client.PostAsJsonAsync("v1/api/route", route2.ToCreateRouteDto());
        create7.StatusCode.Should().Be(HttpStatusCode.Created);

        var create4 = await client.PostAsJsonAsync("v1/api/route/attempt", attempt1.ToCreateRouteAttemptDto());
        create4.StatusCode.Should().Be(HttpStatusCode.Created);
        var create5 = await client.PostAsJsonAsync("v1/api/route/attempt", attempt2.ToCreateRouteAttemptDto());
        create5.StatusCode.Should().Be(HttpStatusCode.Created);
        var create6 = await client.PostAsJsonAsync("v1/api/route/attempt", attempt3.ToCreateRouteAttemptDto());
        create6.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var response = await client.GetAsync($"v1/api/route/attempt?routeId={route2.Id}"); 
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<RouteAttemptDto>>() ?? throw new NullReferenceException();

        content.Count.Should().Be(1);
        content.ElementAt(0).Id.Should().Be(attempt3.Id);
    }

    [Fact]
    public async void Get_Returns200And404()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();
        var summit = new Summit().WithFakeData(user);
        var route = new Route().WithFakeData(user, summit);
        var attempt = new RouteAttempt().WithFakeData(user, route);

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
        var create4 = await client.PostAsJsonAsync("v1/api/route/attempt", attempt.ToCreateRouteAttemptDto());
        create4.StatusCode.Should().Be(HttpStatusCode.Created);
    
        // act & assert
        var response1 = await client.GetAsync($"v1/api/route/attempt/{attempt.Id}"); 
        response1.StatusCode.Should().Be(HttpStatusCode.OK);
        var response2 = await client.GetAsync($"v1/api/route/attempt/{Guid.NewGuid()}");
        response2.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var content = await response1.Content.ReadFromJsonAsync<RouteAttemptDto>() ?? throw new NullReferenceException();

        content.Id.Should().Be(attempt.Id);
    }

    [Fact]
    public async Task Update_Returns204AndIsUpdatedAsync()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();
        var summit = new Summit().WithFakeData(user);
        var route = new Route().WithFakeData(user, summit);
        var attempt = new RouteAttempt().WithFakeData(user, route);

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
        var create4 = await client.PostAsJsonAsync("v1/api/route/attempt", attempt.ToCreateRouteAttemptDto());
        create4.StatusCode.Should().Be(HttpStatusCode.Created);
    
        // act & assert
        var updateDto = new UpdateRouteAttemptDto()
        {
            Name = "test_test_test",
            Description = "test",
            IsPublic = false,
            Coordinates = attempt.Coordinates,
            Time = attempt.Time,
        };

        var response1 = await client.PutAsJsonAsync($"v1/api/route/attempt/{attempt.Id}", updateDto);
        response1.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var response2 = await client.GetAsync($"v1/api/route/attempt/{attempt.Id}");
        response2.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response2.Content.ReadFromJsonAsync<RouteAttemptDto>() ?? throw new NullReferenceException();

        content.Id.Should().Be(attempt.Id);
        content.Name.Should().Be(updateDto.Name);
        content.Description.Should().Be(updateDto.Description);
        content.IsPublic.Should().BeFalse();
    }

    [Fact]
    public async void Delete_Returns204AndIsDeleted()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();
        var summit = new Summit().WithFakeData(user);
        var route = new Route().WithFakeData(user, summit);
        var attempt = new RouteAttempt().WithFakeData(user, route);

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
        var create4 = await client.PostAsJsonAsync("v1/api/route/attempt", attempt.ToCreateRouteAttemptDto());
        create4.StatusCode.Should().Be(HttpStatusCode.Created);
    
        // act & assert
        var response1 = await client.DeleteAsync($"v1/api/route/attempt/{attempt.Id}"); 
        response1.StatusCode.Should().Be(HttpStatusCode.NoContent);
        var response2 = await client.GetAsync($"v1/api/route/attempt/{attempt.Id}");
        response2.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}
