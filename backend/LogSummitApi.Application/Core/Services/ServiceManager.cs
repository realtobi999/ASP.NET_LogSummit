using LogSummitApi.Domain.Core.Interfaces.Factories;
using LogSummitApi.Domain.Core.Interfaces.Services;

namespace LogSummitApi.Application.Core.Services;

public class ServiceManager : IServiceManager
{
    private readonly IServiceFactory _factory;
    private readonly Lazy<IUserService> _users;
    private readonly Lazy<ISummitService> _summit;

    public ServiceManager(IServiceFactory factory)
    {
        _factory = factory;

        // lazy loading
        _users = new(() => _factory.CreateUserService());
        _summit = new (() => _factory.CreateSummitService());
    }

    public IUserService Users => _users.Value;

    public ISummitService Summit => _summit.Value;
}
