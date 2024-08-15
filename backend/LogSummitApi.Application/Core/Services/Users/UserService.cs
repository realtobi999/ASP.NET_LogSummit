using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Exceptions.Common;
using LogSummitApi.Domain.Core.Exceptions.Http;
using LogSummitApi.Domain.Core.Interfaces.Common;
using LogSummitApi.Domain.Core.Interfaces.Repositories;
using LogSummitApi.Domain.Core.Interfaces.Services;

namespace LogSummitApi.Application.Core.Services.Users;

public class UserService : IUserService
{
    private readonly IRepositoryManager _repository;
    private readonly IHasher _hasher;

    public UserService(IRepositoryManager repository, IHasher hasher)
    {
        _repository = repository;
        _hasher = hasher;
    }

    public bool Authenticate(User user, string inputPassword)
    {
        var userPassword = user.Password ?? throw new NullPropertyException(nameof(User), nameof(User.Password));

        if (!_hasher.Compare(inputPassword, userPassword))
        {
            return false;
        }

        return true;
    }

    public async Task CreateAsync(User user)
    {
        _repository.Users.Create(user);
        
        await _repository.SaveSafelyAsync();
    }

    public async Task<User> GetAsync(string email)
    {
        return await _repository.Users.GetAsync(u => u.Email == email) ?? throw new NotFound404Exception(nameof(User), email);
    }
}