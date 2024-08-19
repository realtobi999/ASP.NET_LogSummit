using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Interfaces.Common;
using LogSummitApi.Domain.Core.Interfaces.Repositories;
using LogSummitApi.Domain.Core.Interfaces.Services;

namespace LogSummitApi.Application.Core.Services.Summits.Routes.Attempts;

public class RouteAttemptService : IRouteAttemptService
{
    private readonly IRepositoryManager _repository;
    private readonly IValidator<RouteAttempt> _validator;

    public RouteAttemptService(IRepositoryManager repository, IValidator<RouteAttempt> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task CreateAsync(RouteAttempt attempt)
    {
        // validate the object
        var (valid, exception) = await _validator.IsValidAsync(attempt);
        if (!valid && exception is not null) throw exception;

        _repository.RouteAttempt.Create(attempt);

        await _repository.SaveSafelyAsync(); 
    }
}
