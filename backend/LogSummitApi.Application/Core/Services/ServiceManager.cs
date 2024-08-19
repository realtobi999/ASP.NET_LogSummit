using LogSummitApi.Domain.Core.Interfaces.Factories;
using LogSummitApi.Domain.Core.Interfaces.Services;

namespace LogSummitApi.Application.Core.Services;

public class ServiceManager : IServiceManager
{
    private readonly IServiceFactory _factory;
    private readonly Lazy<IUserService> _users;
    private readonly Lazy<ISummitService> _summits;
    private readonly Lazy<IRouteService> _routes;
    private readonly Lazy<IRouteAttemptService> _routeAttempts;

    public ServiceManager(IServiceFactory factory)
    {
        _factory = factory;

        // lazy loading
        _users = new(() => _factory.CreateUserService());
        _summits = new(() => _factory.CreateSummitService());
        _routes = new(() => _factory.CreateRouteService());
        _routeAttempts = new(() => _factory.CreateRouteAttemptService());
    }

    public IUserService User => _users.Value;
    public ISummitService Summit => _summits.Value;
    public IRouteService Route => _routes.Value;
    public IRouteAttemptService RouteAttempt => _routeAttempts.Value;
}
