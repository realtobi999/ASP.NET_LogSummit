using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Exceptions.HTTP;
using LogSummitApi.Domain.Core.Utilities.Coordinates;
using LogSummitApi.Presentation;
using LogSummitApi.Tests.Helpers.Extensions;
using LogSummitApi.Tests.Integration.Server;

namespace LogSummitApi.Tests.Integration.Endpoints;

public class SummitTests
{
    [Fact]
    public async void CreateSummit_Returns201AndLocationHeader()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();
        var summit = new Summit().WithFakeData(user);

        var create1 = await client.PostAsJsonAsync("v1/api/auth/register", user.ToRegisterUserDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var response = await client.PostAsJsonAsync("v1/api/summit", summit.ToCreateSummitDto());
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var header = response.Headers.GetValues("Location");
        header.Should().Equal(string.Format("/v1/api/summit/{0}", summit.Id));
    }

    [Fact]
    public async void CreateSummit_Returns400WhenThereIsASummitInAProximityRadius()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();
        var summit1 = new Summit().WithFakeData(user);
        var summit2 = new Summit().WithFakeData(user);

        summit1.Coordinate = new Coordinate(45, 90.0000, 100);
        summit2.Coordinate = new Coordinate(45, 90.0005, 100);

        var create1 = await client.PostAsJsonAsync("v1/api/auth/register", user.ToRegisterUserDto());
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
    public async void GetSummitValidCountries_Returns200AndCorrectListOfCountries()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();

        // act & assert
        var response = await client.GetAsync("v1/api/summit/valid-countries");
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<List<string>>() ?? throw new NullReferenceException();

        content.Count.Should().BeInRange(1, 300);
        content.Any(c => c == "Czechia").Should().BeTrue();
    }
}
