using LogSummitApi.Domain.Core.Dto.Users;
using LogSummitApi.Domain.Core.Entities;

namespace LogSummitApi.Domain.Core.Interfaces.Mappers;

public interface IUserMapper
{
    public User CreateEntityFromDto(CreateUserDto dto);
}
