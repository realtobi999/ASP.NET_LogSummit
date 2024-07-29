﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using LogSummitApi.Domain.Core.Exceptions.HTTP;

namespace LogSummitApi.Application.Core.Utilities;

public static class JwtUtils
{
    public static string ParseFromHeader(string? header)
    {
        if (header is null)
        {
            throw new BadRequestException("Authorization header is missing. Expected Format: BEARER <TOKEN>");
        }

        const string bearerPrefix = "Bearer ";
        if (!header.StartsWith(bearerPrefix, StringComparison.OrdinalIgnoreCase))
        {
            throw new BadRequestException("Invalid authorization header format. Expected format: BEARER <TOKEN>");
        }

        string token = header[bearerPrefix.Length..].Trim();

        if (string.IsNullOrEmpty(token))
        {
            throw new BadRequestException("Token is missing in the authorization header.");
        }

        return token;
    }

    public static IEnumerable<Claim> ParsePayload(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var payload = handler.ReadJwtToken(token).Claims;

        return payload;
    }

    public static string? ParseFromPayload(string token, string key)
    {
        var payload = ParsePayload(token);

        return payload.FirstOrDefault(c => c.Type.Equals(key, StringComparison.CurrentCultureIgnoreCase))?.Value;
    }

}
