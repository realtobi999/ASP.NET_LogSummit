using System.Security.Claims;
using GeoCoordinates.Core;
using LogSummitApi.Domain.Core.Dto.Summits;
using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Exceptions.Http;
using LogSummitApi.Presentation;
using LogSummitApi.Tests.Helpers;
using LogSummitApi.Tests.Helpers.Extensions;
using LogSummitApi.Tests.Integration.Server;

namespace LogSummitApi.Tests.Integration.Endpoints;

public class SummitTests
{
    [Fact]
    public async void Create_Returns201AndLocationHeader()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();
        var summit = new Summit().WithFakeData(user);

        var jwt = JwtTestUtils.CreateInstance().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString()),
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"BEARER {jwt}");

        var create1 = await client.PostAsJsonAsync("v1/api/auth/register", user.ToCreateUserDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var response = await client.PostAsJsonAsync("v1/api/summit", summit.ToCreateSummitDto());
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var header = response.Headers.GetValues("Location");
        header.Should().Equal(string.Format("/v1/api/summit/{0}", summit.Id));
    }

    [Fact]
    public async void Create_Returns400WhenThereIsASummitInAProximityRadius()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();
        var summit1 = new Summit().WithFakeData(user);
        var summit2 = new Summit().WithFakeData(user);

        var jwt = JwtTestUtils.CreateInstance().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString()),
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"Bearer {jwt}");

        summit1.Coordinate = new Coordinate(45, 90.0000, 100);
        summit2.Coordinate = new Coordinate(45, 90.0005, 100);

        var create1 = await client.PostAsJsonAsync("v1/api/auth/register", user.ToCreateUserDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("v1/api/summit", summit1.ToCreateSummitDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var response = await client.PostAsJsonAsync("v1/api/summit", summit2.ToCreateSummitDto());
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var content = await response.Content.ReadFromJsonAsync<ErrorMessage>() ?? throw new NullReferenceException();

        content.Title.Should().Be("Bad Request");
        content.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        content.Instance.Should().Be("POST /v1/api/summit");
        content.Type.Should().Be(nameof(BadRequest400Exception));
        content.Detail.Should().Contain("A summit already exists");
    }

    [Fact]
    public async void GetValidCountries_Returns200AndCorrectListOfCountries()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();

        var jwt = JwtTestUtils.CreateInstance().Generate([
            new Claim(ClaimTypes.Role, "User"),
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"BEARER {jwt}");

        // act & assert
        var response = await client.GetAsync("v1/api/summit/valid-countries");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<string>>() ?? throw new NullReferenceException();

        content.Count.Should().BeInRange(1, 300);
        content.Any(c => c == "Czechia").Should().BeTrue();
    }

    [Fact]
    public async void Index_Returns200AndLimitAndOffsetWorks()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();
        var summit1 = new Summit().WithFakeData(user);
        var summit2 = new Summit().WithFakeData(user);
        var summit3 = new Summit().WithFakeData(user);

        var jwt = JwtTestUtils.CreateInstance().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString()),
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"BEARER {jwt}");

        var create1 = await client.PostAsJsonAsync("v1/api/auth/register", user.ToCreateUserDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("v1/api/summit", summit1.ToCreateSummitDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("v1/api/summit", summit2.ToCreateSummitDto());
        create3.StatusCode.Should().Be(HttpStatusCode.Created);
        var create4 = await client.PostAsJsonAsync("v1/api/summit", summit3.ToCreateSummitDto());
        create4.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var limit = 2;
        var offset = 1;

        var response = await client.GetAsync($"v1/api/summit?limit={limit}&offset={offset}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<SummitDto>>() ?? throw new NullReferenceException();

        content.Count.Should().Be(limit);
        content.ElementAt(0).Id.Should().Be(summit2.Id);
        content.ElementAt(1).Id.Should().Be(summit3.Id);
    }

    [Fact]
    public async void Index_WorksWithUserFilter()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user1 = new User().WithFakeData();
        var user2 = new User().WithFakeData();
        var summit1 = new Summit().WithFakeData(user1);
        var summit2 = new Summit().WithFakeData(user1);
        var summit3 = new Summit().WithFakeData(user2);

        var jwt1 = JwtTestUtils.CreateInstance().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user1.Id.ToString()),
        ]);
        var jwt2 = JwtTestUtils.CreateInstance().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user2.Id.ToString()),
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"BEARER {jwt2}");

        var create1 = await client.PostAsJsonAsync("v1/api/auth/register", user1.ToCreateUserDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("v1/api/auth/register", user2.ToCreateUserDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);

        var create5 = await client.PostAsJsonAsync("v1/api/summit", summit3.ToCreateSummitDto());
        create5.StatusCode.Should().Be(HttpStatusCode.Created);

        client.DefaultRequestHeaders.Remove("Authorization");
        client.DefaultRequestHeaders.Add("Authorization", $"BEARER {jwt1}");

        var create3 = await client.PostAsJsonAsync("v1/api/summit", summit1.ToCreateSummitDto());
        create3.StatusCode.Should().Be(HttpStatusCode.Created);
        var create4 = await client.PostAsJsonAsync("v1/api/summit", summit2.ToCreateSummitDto());
        create4.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var response = await client.GetAsync($"v1/api/summit/user/{user1.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<SummitDto>>() ?? throw new NullReferenceException();

        content.Count.Should().Be(2);
        content.ElementAt(0).Id.Should().Be(summit1.Id);
        content.ElementAt(1).Id.Should().Be(summit2.Id);
    }

    [Fact]
    public async void Index_WorksWithCountryFilter()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();
        var summit1 = new Summit().WithFakeData(user);
        var summit2 = new Summit().WithFakeData(user);

        summit1.Country = "Hungary";

        var jwt = JwtTestUtils.CreateInstance().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user.Id.ToString()),
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"BEARER {jwt}");

        var create1 = await client.PostAsJsonAsync("v1/api/auth/register", user.ToCreateUserDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("v1/api/summit", summit1.ToCreateSummitDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("v1/api/summit", summit2.ToCreateSummitDto());
        create3.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var response = await client.GetAsync($"v1/api/summit/country/hungary");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<SummitDto>>() ?? throw new NullReferenceException();

        content.Count.Should().Be(1);
        content.ElementAt(0).Id.Should().Be(summit1.Id);
    }

    [Fact]
    public async void Get_Returns200And404()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();
        var summit = new Summit().WithFakeData(user);

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
        var response1 = await client.GetAsync($"v1/api/summit/{new Summit().WithFakeData(user).Id}");
        response1.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var response2 = await client.GetAsync($"v1/api/summit/{summit.Id}");
        response2.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response2.Content.ReadFromJsonAsync<SummitDto>() ?? throw new NullReferenceException();

        content.Id.Should().Be(summit.Id);
    }

    [Fact]
    public async void Update_Returns204AndIsUpdated()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();
        var summit = new Summit().WithFakeData(user);

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
        var updateDto = new UpdateSummitDto()
        {
            Name = "test",
            Description = "test_test_test_test_test",
            Country = summit.Country,
            IsPublic = summit.IsPublic,
            Coordinate = summit.Coordinate,
        };

        var response = await client.PutAsJsonAsync($"v1/api/summit/{summit.Id}", updateDto);
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var get = await client.GetAsync($"v1/api/summit/{summit.Id}");
        get.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await get.Content.ReadFromJsonAsync<SummitDto>() ?? throw new NullReferenceException();

        content.Id.Should().Be(summit.Id);
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
        var response = await client.DeleteAsync($"v1/api/summit/{summit.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var get = await client.GetAsync($"v1/api/summit/{summit.Id}");
        get.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async void Get_ReturnsNoContentWhenSummitIsPrivate()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user1 = new User().WithFakeData();
        var user2 = new User().WithFakeData();
        var summit = new Summit().WithFakeData(user1);

        summit.IsPublic = false;

        var jwt1 = JwtTestUtils.CreateInstance().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user1.Id.ToString()),
        ]);
        client.DefaultRequestHeaders.Add("Authorization", $"BEARER {jwt1}");

        var create1 = await client.PostAsJsonAsync("v1/api/auth/register", user1.ToCreateUserDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);
        var create3 = await client.PostAsJsonAsync("v1/api/auth/register", user2.ToCreateUserDto());
        create3.StatusCode.Should().Be(HttpStatusCode.Created);
        var create2 = await client.PostAsJsonAsync("v1/api/summit", summit.ToCreateSummitDto());
        create2.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var jwt2 = JwtTestUtils.CreateInstance().Generate([
            new Claim(ClaimTypes.Role, "User"),
            new Claim("UserId", user2.Id.ToString()),
        ]);
        client.DefaultRequestHeaders.Remove("Authorization");
        client.DefaultRequestHeaders.Add("Authorization", $"BEARER {jwt2}");

        var response = await client.GetAsync($"v1/api/summit/{summit.Id}");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
