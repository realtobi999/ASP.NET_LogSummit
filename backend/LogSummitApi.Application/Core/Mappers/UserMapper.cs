using LogSummitApi.Domain.Core.Dto.Users;
using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Interfaces.Common;
using LogSummitApi.Domain.Core.Interfaces.Mappers;

namespace LogSummitApi.Application.Core.Mappers;

public class UserMapper : IUserMapper
{
    private readonly IHasher _hasher;

    public UserMapper(IHasher hasher)
    {
        _hasher = hasher;
    }

    public User CreateEntityFromDto(CreateUserDto dto)
    {
        return new User       
        {
            Id = dto.Id ?? Guid.NewGuid(),
            Username = dto.Username,
            Email = dto.Email,
            Password = _hasher.Hash(dto.Password),
        };
    }
}
