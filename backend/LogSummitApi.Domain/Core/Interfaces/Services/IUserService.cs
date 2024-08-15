using LogSummitApi.Domain.Core.Dto.Users;
using LogSummitApi.Domain.Core.Entities;

namespace LogSummitApi.Domain.Core.Interfaces.Services;

public interface IUserService
{
    Task<User> GetAsync(string email);
    bool Authenticate(User user, string inputPassword);
    Task<User> CreateAsync(RegisterUserDto RegisterUserDto);
}
