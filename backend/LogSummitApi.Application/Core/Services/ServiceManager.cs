using LogSummitApi.Domain.Core.Interfaces.Factories;
using LogSummitApi.Domain.Core.Interfaces.Services;

namespace LogSummitApi.Application.Core.Services;

public class ServiceManager : IServiceManager
{
    private readonly IServiceFactory _factory;
    private readonly Lazy<IUserService> _users;
    private readonly Lazy<ISummitService> _summits;
    private readonly Lazy<ISummitPushService> _summitPushes;

    public ServiceManager(IServiceFactory factory)
    {
        _factory = factory;

        // lazy loading
        _users = new(() => _factory.CreateUserService());
        _summits = new (() => _factory.CreateSummitService());
        _summitPushes = new (() => _factory.CreateSummitPushService());
    }

    public IUserService User => _users.Value;
    public ISummitService Summit => _summits.Value;
    public ISummitPushService SummitPush => _summitPushes.Value;
}
