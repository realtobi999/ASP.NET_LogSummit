using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using LogSummitApi.Domain.Core.Interfaces.Common;
using Microsoft.IdentityModel.Tokens;

namespace LogSummitApi.Application.Core.Utilities;

public class Jwt : IJwt
{
    private readonly string _key;
    private readonly string _issuer;

    public Jwt(string issuer, string key)
    {
        _key = key;
        _issuer = issuer;
    }

    public string Issuer => this._issuer;

    public string Key => this._key; 

    public string Generate()
    {
        var descriptor = new SecurityTokenDescriptor
        {
            Expires = DateTime.UtcNow.AddMinutes(30),
            Issuer = Issuer,
            Audience = Issuer,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_key)), SecurityAlgorithms.HmacSha256Signature)
        };

        return this.Generate(descriptor);
    }

    public string Generate(IEnumerable<Claim> claims)
    {
        var descriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(30),
            Issuer = Issuer,
            Audience = Issuer,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_key)), SecurityAlgorithms.HmacSha256Signature)
        };

        return this.Generate(descriptor);
    }
    
    public string Generate(SecurityTokenDescriptor descriptor)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(descriptor);

        return tokenHandler.WriteToken(token);
    }
}
