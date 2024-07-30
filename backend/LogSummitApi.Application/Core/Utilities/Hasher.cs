using System.Security.Cryptography;
using LogSummitApi.Domain.Core.Interfaces.Utilities;

namespace LogSummitApi.Application.Core.Utilities;

public class Hasher : IHasher
{
    private const int _saltSize = 128 / 8;  // Size of the salt in bytes
    private const int _hashSize = 256 / 8;  // Size of the hash in bytes
    private const int _iterations = 10_000; // Number of iterations for PBKDF2
    private static readonly HashAlgorithmName _algorithm = HashAlgorithmName.SHA256;

    private static readonly char Delimiter = ';';

    public bool Compare(string plainText, string hashedText)
    {
        var parts = hashedText.Split(Delimiter);
        if (parts.Length != 2)
        {
            throw new FormatException("Invalid hash format.");
        }

        var salt = Convert.FromBase64String(parts[0]);
        var hash = Convert.FromBase64String(parts[1]);

        var hashOfInput = Rfc2898DeriveBytes.Pbkdf2(plainText, salt, _iterations, _algorithm, _hashSize);

        return CryptographicOperations.FixedTimeEquals(hash, hashOfInput);
    }

    public string Hash(string plainText)
    {
        var salt = RandomNumberGenerator.GetBytes(_saltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(plainText, salt, _iterations, _algorithm, _hashSize);

        return string.Join(Delimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
    }
}
