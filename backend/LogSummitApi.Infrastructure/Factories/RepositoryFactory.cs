using LogSummitApi.Domain.Core.Interfaces.Factories;
using LogSummitApi.Domain.Core.Interfaces.Repositories;
using LogSummitApi.Domain.Core.Interfaces.Repositories.HTTP;
using LogSummitApi.Infrastructure.HTTP.Repositories;
using LogSummitApi.Infrastructure.Persistance;
using LogSummitApi.Infrastructure.Persistance.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace LogSummitApi.Infrastructure.Factories;

public class RepositoryFactory : IRepositoryFactory
{
    private readonly LogSummitContext _context;
    private readonly IHttpClientFactory _httpFactory;
    private readonly IMemoryCache _cache;

    public RepositoryFactory(LogSummitContext context, IHttpClientFactory httpFactory, IMemoryCache cache)
    {
        _context = context;
        _httpFactory = httpFactory;
        _cache = cache;
    }

    public IHttpCountryRepository CreateHttpCountryRepository()
    {
        return new HttpCountryRepository(_httpFactory, _cache);
    }

    public ISummitRepository CreateSummitRepository()
    {
        return new SummitRepository(_context);
    }

    public IUserRepository CreateUserRepository()
    {
        return new UserRepository(_context);
    }
}
