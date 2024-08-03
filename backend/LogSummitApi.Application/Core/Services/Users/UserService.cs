using LogSummitApi.Domain.Core.Dto.User;
using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Exceptions.HTTP;
using LogSummitApi.Domain.Core.Interfaces.Repositories;
using LogSummitApi.Domain.Core.Interfaces.Services;
using LogSummitApi.Domain.Core.Interfaces.Utilities;

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
        var userPassword = user.Password ?? throw new NullReferenceException(user.Password);

        if (!_hasher.Compare(inputPassword, userPassword))
        {
            return false;
        }

        return true;
    }

    public async Task<bool> Authenticate(string email, string inputPassword)
    {
        var user = await Get(email);

        return this.Authenticate(user, inputPassword);
    }

    public async Task<User> Create(RegisterUserDto registerUserDto)
    {
        var user = new User()
        {
            Id = registerUserDto.Id ?? Guid.NewGuid(),
            Username = registerUserDto.Username,
            Email = registerUserDto.Email,
            Password = _hasher.Hash(registerUserDto.Password),
        };

        _repository.Users.Create(user);
        await _repository.SaveSafelyAsync();

        return user;
    }

    public async Task<User> Get(string email)
    {
        return await _repository.Users.Get(u => u.Email == email) ?? throw new NotFound404Exception(nameof(User), email);
    }
}