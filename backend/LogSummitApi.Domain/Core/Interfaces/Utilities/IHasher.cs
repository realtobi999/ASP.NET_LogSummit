namespace LogSummitApi.Domain.Core.Interfaces.Utilities;

public interface IHasher
{
    string Hash(string plainText);
    bool Compare(string plainText, string hashedText);
}
