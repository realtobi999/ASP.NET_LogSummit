using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Presentation;
using LogSummitApi.Tests.Integration.Server;

namespace LogSummitApi.Tests.Integration.Endpoints;

public class UtilControllerTests
{
    [Fact]
    public async void HealthCheck_ReturnsStatusCode200()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();

        // act & assert
        var response = await client.GetAsync("v1/api/check/health");
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async void ErrorCheck_ReturnsErrorMessage()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();

        // act & assert
        var response = await client.GetAsync("v1/api/check/error");
        response.StatusCode.Should().Be(HttpStatusCode.InternalServerError);

        var content = await response.Content.ReadFromJsonAsync<ErrorMessage>() ?? throw new NullReferenceException();

        content.StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        content.Instance.Should().Be("GET /v1/api/check/error");
    }

    [Fact]
    public async void HealthCheck_ReturnsStatusCode401()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();

        // act & assert
        var response = await client.GetAsync("v1/api/check/auth");
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
