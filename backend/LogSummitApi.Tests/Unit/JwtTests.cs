using System.IdentityModel.Tokens.Jwt;
using FluentAssertions;
using LogSummitApi.Application.Core.Utilities;
using LogSummitApi.Tests.Helpers;
using Xunit;

namespace LogSummitApi.Tests.Unit;

public class JwtTests
{
    [Fact]
    public void Jwt_Generate_Works()
    {
        // prepare
        var issuer = "TEST_ISSUER";
        var key = JwtTestUtils.GenerateRandomKey(); 
        var jwt = new Jwt(issuer, key);

        // act & assert
        var token = jwt.Generate();

        token.Should().NotBeNull();

        var tokenData = new JwtSecurityTokenHandler().ReadJwtToken(token);

        tokenData.Issuer.Should().Be(issuer);
    }
}
