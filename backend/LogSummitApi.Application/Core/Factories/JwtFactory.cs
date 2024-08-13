using LogSummitApi.Application.Core.Utilities;
using LogSummitApi.Domain.Core.Interfaces.Common;
using Microsoft.Extensions.Configuration;

namespace LogSummitApi.Application.Core.Factories;

public class JwtFactory
{
    public static IJwt Create(string? issuer, string? key)
    {
        if (string.IsNullOrEmpty(issuer))
        {
            throw new NullReferenceException("JWT Issuer configuration is missing");
        }
        if (string.IsNullOrEmpty(key))
        {
            throw new NullReferenceException("JWT Key configuration is missing");
        }

        return new Jwt(issuer, key);
    }
}
