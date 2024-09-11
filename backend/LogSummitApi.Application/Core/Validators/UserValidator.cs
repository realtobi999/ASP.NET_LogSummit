using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Exceptions.Http;
using LogSummitApi.Domain.Core.Interfaces.Common;
using LogSummitApi.Domain.Core.Interfaces.Repositories;

namespace LogSummitApi.Application.Core.Validators;

public class UserValidator : IValidator<User>
{
    private readonly IRepositoryManager _repository;

    public UserValidator(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task<(bool isValid, Exception? exception)> IsValidAsync(User user)
    {
        var users = await _repository.Users.IndexAsync();

        if (users.Any(existingUser => existingUser.Email == user.Email))
        {
            return (false, new BadRequest400Exception("The email address provided is already in use. Please use a different email address."));
        }

        return (true, null);
    }
}
