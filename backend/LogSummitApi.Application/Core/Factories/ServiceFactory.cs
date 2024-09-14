using LogSummitApi.Application.Core.Services.Summits;
using LogSummitApi.Application.Core.Services.Summits.Routes;
using LogSummitApi.Application.Core.Services.Summits.Routes.Attempts;
using LogSummitApi.Application.Core.Services.Users;
using LogSummitApi.Domain.Core.Interfaces.Common;
using LogSummitApi.Domain.Core.Interfaces.Factories;
using LogSummitApi.Domain.Core.Interfaces.Repositories;
using LogSummitApi.Domain.Core.Interfaces.Services;

namespace LogSummitApi.Application.Core.Factories;

public class ServiceFactory : IServiceFactory
{
    private readonly IRepositoryManager _repository;
    private readonly IHasher _hasher;
    private readonly IValidatorFactory _validators;

    public ServiceFactory(IRepositoryManager repository, IHasher hasher, IValidatorFactory validators)
    {
        _repository = repository;
        _hasher = hasher;
        _validators = validators;
    }

    public IRouteAttemptService CreateRouteAttemptService()
    {
        return new RouteAttemptService(_repository, _validators.CreateRouteAttemptValidator());
    }

    public IRouteService CreateRouteService()
    {
        return new RouteService(_repository, _validators.CreateRouteValidator());
    }

    public ISummitService CreateSummitService()
    {
        return new SummitService(_repository, _validators.CreateSummitValidator());
    }

    public IUserService CreateUserService()
    {
        return new UserService(_repository, _hasher, _validators.CreateUserValidator());
    }
}
