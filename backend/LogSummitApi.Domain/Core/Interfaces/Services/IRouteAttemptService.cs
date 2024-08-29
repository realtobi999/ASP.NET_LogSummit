using LogSummitApi.Domain.Core.Entities;

namespace LogSummitApi.Domain.Core.Interfaces.Services;

public interface IRouteAttemptService
{
    Task<IEnumerable<RouteAttempt>> IndexAsync();
    Task CreateAsync(RouteAttempt attempt);
}
