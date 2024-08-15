using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LogSummitApi.Domain.Core.Dto.Users;
using LogSummitApi.Domain.Core.Interfaces.Common;

namespace LogSummitApi.Domain.Core.Entities;

public class User : ISerializable<UserDto>
{
    [Required, Column("id")]
    public Guid Id { get; set; }

    [Required, Column("username"), MaxLength(155)]
    public string? Username { get; set; }

    [Required, Column("email"), EmailAddress, MaxLength(155)]
    public string? Email { get; set; }

    [Required, Column("password"), MinLength(8), MaxLength(155)]
    public string? Password { get; set; }

    // entity relationships

    public ICollection<Summit>? Summits { get; set; }
    public ICollection<Route>? Routes { get; set; }

    // methods

    public UserDto ToDto()
    {
        return new UserDto()
        {
            Id = this.Id,
            Email = this.Email,
        };
    }
}
