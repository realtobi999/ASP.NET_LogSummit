using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Presentation;
using LogSummitApi.Tests.Helpers.Extensions;
using LogSummitApi.Tests.Integration.Server;

namespace LogSummitApi.Tests.Integration.Endpoints;

public class AuthTests
{
    [Fact]
    public async void RegisterUser_Returns200AndLocationHeader()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();

        // act & assert
        var response = await client.PostAsJsonAsync("/v1/api/auth/register", user.ToRegisterUserDto());
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var header = response.Headers.GetValues("Location");
        header.Should().Equal(string.Format("/v1/api/user/{0}", user.Id));
    }
}
