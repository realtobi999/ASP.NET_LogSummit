namespace LogSummitApi.Domain.Core.Interfaces.Common;

public interface IHasher
{
    string Hash(string? plainText);
    bool Compare(string plainText, string hashedText);
}
