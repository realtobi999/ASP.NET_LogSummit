using LogSummitApi.Domain.Core.Interfaces.Factories;
using LogSummitApi.Domain.Core.Interfaces.Services;

namespace LogSummitApi.Application.Core.Services;

public class ServiceManager : IServiceManager
{
    private readonly IServiceFactory _factory;
    private readonly Lazy<IUserService> _users;

    public ServiceManager(IServiceFactory factory)
    {
        _factory = factory;

        // lazy loading
        _users = new(() => _factory.CreateUserService());
    }

    public IUserService Users => _users.Value;
}
