﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using LogSummitApi.Domain.Core.Dto.Users;

namespace LogSummitApi.Domain.Core.Entities;

public class User
{
    [Required, Column("id")]
    public Guid Id { get; set; }

    [Required, Column("email"), EmailAddress, MaxLength(155)]
    public string? Email { get; set; }

    [Required, Column("password"), MinLength(8), MaxLength(155)]
    public string? Password { get; set; }

    public UserDto ToDto()
    {
        return new UserDto()
        {
            Id = this.Id,
            Email = this.Email,
            Password = this.Password,
        };
    }
}
