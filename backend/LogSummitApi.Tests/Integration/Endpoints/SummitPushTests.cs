using System.Security.Claims;
using LogSummitApi.Domain.Core.Dto.Summit.Pushes;
using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Exceptions.Http;
using LogSummitApi.Domain.Core.Utilities.Coordinates;
using LogSummitApi.Presentation;
using LogSummitApi.Tests.Helpers;
using LogSummitApi.Tests.Helpers.Extensions;
using LogSummitApi.Tests.Integration.Server;

namespace LogSummitApi.Tests.Integration.Endpoints;

public class SummitPushTests
{
    [Fact]
    public async void Create_Returns200AndLocationHeader()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();
        var summit = new Summit().WithFakeData(user);
        var summitPush = new SummitPush().WithFakeData(user, summit);

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
        var response = await client.PostAsJsonAsync("v1/api/summit/push", summitPush.ToCreateSummitPushDto());
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var header = response.Headers.GetValues("Location");
        header.Should().Equal(string.Format("/v1/api/summit/push/{0}", summitPush.Id));
    }

    [Fact]
    public async void Create_Returns400BecauseTheLastCoordinateIsOutsideOfTheSummitRange()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();
        var summit = new Summit().WithFakeData(user);
        var summitPush = new SummitPush().WithFakeData(user, summit);

        // here we set the last summit push coordinate something entirely different than the summit coordinate
        summitPush.Coordinates[^1] = new Coordinate(summit.Coordinate!.Latitude * 0.5, summit.Coordinate!.Longitude * 0.5, 100);

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
        var response = await client.PostAsJsonAsync("v1/api/summit/push", summitPush.ToCreateSummitPushDto());
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ErrorMessage>() ?? throw new NullReferenceException();

        content.Title.Should().Be("Bad Request");
        content.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        content.Instance.Should().Be("POST /v1/api/summit/push");
        content.Type.Should().Be(nameof(BadRequest400Exception));
        content.Detail.Should().Contain(Summit.SummitPushRadius.ToString());
    }

    [Fact]
    public async void Index_Returns200AndLimitAndOffsetWorks()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();
        var summit = new Summit().WithFakeData(user);
        var summitPush1 = new SummitPush().WithFakeData(user, summit);
        var summitPush2 = new SummitPush().WithFakeData(user, summit);
        var summitPush3 = new SummitPush().WithFakeData(user, summit);

        var jwt = JwtTestUtils.CreateInstance().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString()),
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"BEARER {jwt}");

        var create1 = await client.PostAsJsonAsync("v1/api/auth/register", user.ToRegisterUserDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("v1/api/summit", summit.ToCreateSummitDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);

        var create3 = await client.PostAsJsonAsync("v1/api/summit/push", summitPush1.ToCreateSummitPushDto());
        create3.StatusCode.Should().Be(HttpStatusCode.Created);
        var create4 = await client.PostAsJsonAsync("v1/api/summit/push", summitPush2.ToCreateSummitPushDto());
        create4.StatusCode.Should().Be(HttpStatusCode.Created);
        var create5 = await client.PostAsJsonAsync("v1/api/summit/push", summitPush3.ToCreateSummitPushDto());
        create5.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var offset = 1;
        var limit = 2;

        var response = await client.GetAsync($"v1/api/summit/push?limit={limit}&offset={offset}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<SummitPushDto>>() ?? throw new NullReferenceException();

        content.Count.Should().Be(limit);
        content.ElementAt(0).Id.Should().Be(summitPush2.Id);
    }
}
