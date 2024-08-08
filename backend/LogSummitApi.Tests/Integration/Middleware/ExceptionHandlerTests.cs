using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Presentation;
using LogSummitApi.Tests.Integration.Server;

namespace LogSummitApi.Tests.Integration.Middleware;

public class ExceptionHandlerTests
{
    [Fact]
    public async void WorksAndReturnsHttpException()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();

        // act & assert
        var response = await client.GetAsync($"v1/api/summit/{Guid.NewGuid()}");
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
        
        var content = await response.Content.ReadFromJsonAsync<ErrorMessage>() ?? throw new NullReferenceException(); 

        content.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        content.Title.Should().Be("Resource Not Found");
    }
}