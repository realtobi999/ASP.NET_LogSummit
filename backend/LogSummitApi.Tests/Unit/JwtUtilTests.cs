using System.Security.Claims;
using LogSummitApi.Application.Core.Utilities;
using LogSummitApi.Domain.Core.Exceptions.Http;
using LogSummitApi.Tests.Helpers;

namespace LogSummitApi.Tests.Unit;

public class JwtUtilTests
{
    [Fact]
    public void ParsePayload_Works()
    {
        // prepare
        var issuer = "TEST_ISSUER";
        var key = JwtTestUtils.GenerateRandomKey();
        var jwt = new Jwt(issuer, key);

        // act & assert
        var token = jwt.Generate([
            new Claim("AccountId", "123")
        ]);

        token.Should().NotBeNull();

        var payload = JwtUtils.ParsePayload(token);
        payload.Count().Should().BeGreaterThan(0);
        payload.ElementAt(0).Type.Should().Be("AccountId");
        payload.ElementAt(0).Value.Should().Be("123");
    }

    [Fact]
    public void Parse_ValidationWorks()
    {
        // act & assert
        Assert.Throws<BadRequest400Exception>(() => { JwtUtils.ParseFromHeader(""); });
        Assert.Throws<BadRequest400Exception>(() => { JwtUtils.ParseFromHeader("Bearer"); });
        Assert.Throws<BadRequest400Exception>(() => { JwtUtils.ParseFromHeader("Bearer "); });
        Assert.Throws<BadRequest400Exception>(() => { JwtUtils.ParseFromHeader("TOKEN"); });
    }

    [Fact]
    public void Parse_Works()
    {
        // prepare
        var issuer = "TEST_ISSUER";
        var key = JwtTestUtils.GenerateRandomKey();
        var jwt = new Jwt(issuer, key);

        var token = jwt.Generate();

        token.Should().NotBeNull();

        // act & assert
        JwtUtils.ParseFromHeader(string.Format("Bearer {0}", token)).Should().Be(token);
    }

    [Fact]
    public void ParseFromPayload_Works()
    {
        // prepare
        var issuer = "TEST_ISSUER";
        var key = JwtTestUtils.GenerateRandomKey();
        var jwt = new Jwt(issuer, key);

        var token = jwt.Generate([
            new Claim("AccountId", "123")
        ]);
        token.Should().NotBeNull();

        // act & assert
        JwtUtils.ParseFromPayload(token, "AccountId").Should().Be("123");
        JwtUtils.ParseFromPayload(token, "ACCOUNTID").Should().Be("123");
        JwtUtils.ParseFromPayload(token, "accountid").Should().Be("123");

        JwtUtils.ParseFromPayload(token, "account_id").Should().BeNull();
    }
}
