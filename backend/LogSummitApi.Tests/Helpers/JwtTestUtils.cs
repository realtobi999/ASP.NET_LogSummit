using System.Security.Cryptography;
using LogSummitApi.Application.Core.Utilities;
using Microsoft.Extensions.Configuration;

namespace LogSummitApi.Tests.Helpers;

public static class JwtTestUtils
{
    public static string GenerateRandomKey()
    {
        byte[] key = new byte[24];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(key);
        }
        return Convert.ToBase64String(key);
    }

    public static Jwt CreateInstance()
    {
        var configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                      .AddJsonFile("appsettings.json")
                                                      .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true)
                                                      .Build();

        var jwtIssuer = configuration.GetSection("Jwt:Issuer").Value;
        var jwtKey = configuration.GetSection("Jwt:Key").Value;

        if (string.IsNullOrEmpty(jwtIssuer))
        {
            throw new NullReferenceException("JWT Issuer configuration is missing");
        }
        if (string.IsNullOrEmpty(jwtKey))
        {
            throw new NullReferenceException("JWT Key configuration is missing");
        }

        return new Jwt(jwtIssuer, jwtKey);
    }
}
