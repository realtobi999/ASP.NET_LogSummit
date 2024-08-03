using LogSummitApi.Domain.Core.Dto.User;
using LogSummitApi.Domain.Core.Entities;

namespace LogSummitApi.Domain.Core.Interfaces.Services;

public interface IUserService
{
    Task<User> Get(string email);
    Task<bool> Authenticate(string email, string inputPassword);
    bool Authenticate(User user, string inputPassword);
    Task<User> Create(RegisterUserDto RegisterUserDto);
}
