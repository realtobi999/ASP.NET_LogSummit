using System.Security.Cryptography;

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
}
