using LogSummitApi.Application.Core.Services.Users;
using LogSummitApi.Domain.Core.Interfaces.Factories;
using LogSummitApi.Domain.Core.Interfaces.Repositories;
using LogSummitApi.Domain.Core.Interfaces.Services;
using LogSummitApi.Domain.Core.Interfaces.Utilities;

namespace LogSummitApi.Application.Core.Factories;

public class ServiceFactory : IServiceFactory
{
    private readonly IRepositoryManager _repository;
    private readonly IHasher _hasher;

    public ServiceFactory(IRepositoryManager repository, IHasher hasher)
    {
        _repository = repository;
        _hasher = hasher;
    }

    public IUserService CreateUserService()
    {
        return new UserService(_repository, _hasher);
    }
}
