using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Interfaces.Repositories;
using LogSummitApi.Domain.Core.Interfaces.Services;

namespace LogSummitApi.Application.Core.Services.Summits.Routes.Attempts;

public class RouteAttemptService : IRouteAttemptService
{
    private readonly IRepositoryManager _repository;

    public RouteAttemptService(IRepositoryManager repository)
    {
        _repository = repository;
    }

    public async Task CreateAsync(RouteAttempt attempt)
    {
        _repository.RouteAttempt.Create(attempt);

        await _repository.SaveSafelyAsync(); 
    }
}
