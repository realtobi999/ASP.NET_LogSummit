using LogSummitApi.Domain.Core.Dto.Users;
using LogSummitApi.Domain.Core.Entities;
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

    public async Task<User> Create(RegisterUserDto RegisterUserDto)
    {
        var user = new User()
        {
            Id = RegisterUserDto.Id ?? Guid.NewGuid(),
            Email = RegisterUserDto.Email,
            Password = _hasher.Hash(RegisterUserDto.Password),
        };

        _repository.Users.Create(user);
        await _repository.SaveSafelyAsync();

        return user;
    }
}