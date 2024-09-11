using LogSummitApi.Application.Core.Validators;
using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Interfaces.Common;
using LogSummitApi.Domain.Core.Interfaces.Factories;
using LogSummitApi.Domain.Core.Interfaces.Repositories;

namespace LogSummitApi.Application.Core.Factories;

public class ValidatorFactory : IValidatorFactory
{
    IRepositoryManager _repository;

    public ValidatorFactory(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public IValidator<RouteAttempt> CreateRouteAttemptValidator()
    {
        return new RouteAttemptValidator(_repository);
    }

    public IValidator<Route> CreateRouteValidator()
    {
        return new RouteValidator(_repository);
    }

    public IValidator<Summit> CreateSummitValidator()
    {
        return new SummitValidator(_repository);
    }

    public IValidator<User> CreateUserValidator()
    {
        return new UserValidator(_repository);
    }
}
