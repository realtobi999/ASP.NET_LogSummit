using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace LogSummitApi.Domain.Core.Interfaces.Utilities;

public interface IJwt
{
    string Issuer { get; }
    string Key { get; }
    string Generate();
    string Generate(IEnumerable<Claim> claims);
    string Generate(SecurityTokenDescriptor descriptor);
}
