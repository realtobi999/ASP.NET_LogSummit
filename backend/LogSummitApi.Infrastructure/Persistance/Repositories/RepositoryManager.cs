using LogSummitApi.Domain.Core.Exceptions;
using LogSummitApi.Domain.Core.Interfaces.Factories;
using LogSummitApi.Domain.Core.Interfaces.Repositories;
using LogSummitApi.Domain.Core.Interfaces.Repositories.HTTP;

namespace LogSummitApi.Infrastructure.Persistance.Repositories;

public class RepositoryManager : IRepositoryManager
{
    private readonly LogSummitContext _context;
    private readonly IRepositoryFactory _factory;
    private readonly IHttpRepositoryFactory _httpFactory;
    
    private readonly Lazy<IUserRepository> _users;
    private readonly Lazy<ISummitRepository> _summits;
    private readonly Lazy<IHttpCountryRepository> _httpCountries;
    private readonly Lazy<IRouteRepository> _routes;

    public RepositoryManager(LogSummitContext context, IRepositoryFactory factory, IHttpRepositoryFactory httpFactory)
    {
        _context = context;
        _factory = factory;
        _httpFactory = httpFactory;

        // lazy loading
        _users = new(() => _factory.CreateUserRepository());
        _summits = new(() => _factory.CreateSummitRepository());
        _httpCountries = new(() => _httpFactory.CreateCountryHttpRepository());
        _routes = new(() => _factory.CreateRouteRepository());
    }

    public IUserRepository Users => _users.Value;

    public ISummitRepository Summit => _summits.Value;

    public IHttpCountryRepository HttpCountry => _httpCountries.Value;

    public IRouteRepository Route => _routes.Value;

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task SaveSafelyAsync()
    {
        var affected = await this.SaveAsync();

        if (affected == 0)
            throw new ZeroRowsAffectedException();
    }
}
