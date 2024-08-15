using LogSummitApi.Application.Core.Utilities;
using LogSummitApi.Domain.Core.Dto.User;
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

    [Fact]
    public async void LoginUser_Returns200AndJwt()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();

        var create1 = await client.PostAsJsonAsync("/v1/api/auth/register", user.ToRegisterUserDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var loginDto = new LoginUserDto()
        {
            Email = user.Email,
            Password = user.Password,
        };

        var response = await client.PostAsJsonAsync("/v1/api/auth/login", loginDto);
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var content = await response.Content.ReadFromJsonAsync<LoginResponseDto>() ?? throw new NullReferenceException();
        content.Token.Should().NotBeNull();
        content.User.Should().NotBeNull();
        content.User!.Id.Should().Be(user.Id);

        JwtUtils.ParseFromPayload(content.Token!, "UserId").Should().Be(user.Id.ToString());
    }

    [Fact]
    public async void LoginUser_Returns401WhenTryingToAuthenticateWithBadPassword()
    {
        // prepare
        var client = new WebAppFactory<Program>().CreateDefaultClient();
        var user = new User().WithFakeData();

        var create1 = await client.PostAsJsonAsync("/v1/api/auth/register", user.ToRegisterUserDto());
        create1.StatusCode.Should().Be(HttpStatusCode.Created);

        // act & assert
        var loginDto = new LoginUserDto()
        {
            Email = user.Email,
            Password = "WRONG_PASSWORD",
        };

        var response = await client.PostAsJsonAsync("/v1/api/auth/login", loginDto);
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
