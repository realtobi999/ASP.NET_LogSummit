using LogSummitApi.Domain.Core.Entities;

namespace LogSummitApi.Domain.Core.Interfaces.Services;

public interface IUserService
{
    Task<User> GetAsync(string email);
    Task CreateAsync(User user);
    bool Authenticate(User user, string inputPassword);
}
