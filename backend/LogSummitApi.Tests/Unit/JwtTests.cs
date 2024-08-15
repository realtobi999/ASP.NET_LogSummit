using System.IdentityModel.Tokens.Jwt;
using LogSummitApi.Application.Core.Utilities;
using LogSummitApi.Tests.Helpers;

namespace LogSummitApi.Tests.Unit;

public class JwtTests
{
    [Fact]
    public void Generate_WorksAndReturnsAToken()
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
