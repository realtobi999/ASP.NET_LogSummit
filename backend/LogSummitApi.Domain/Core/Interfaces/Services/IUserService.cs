using LogSummitApi.Domain.Core.Dto.Users;
using LogSummitApi.Domain.Core.Entities;

namespace LogSummitApi.Domain.Core.Interfaces.Services;

public interface IUserService
{
    Task<User> Create(RegisterUserDto RegisterUserDto);
}
