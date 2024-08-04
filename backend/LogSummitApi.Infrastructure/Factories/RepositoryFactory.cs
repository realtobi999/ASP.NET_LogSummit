using LogSummitApi.Domain.Core.Interfaces.Factories;
using LogSummitApi.Domain.Core.Interfaces.Repositories;
using LogSummitApi.Domain.Core.Interfaces.Repositories.HTTP;
using LogSummitApi.Infrastructure.HTTP.Repositories;
using LogSummitApi.Infrastructure.Persistance;
using LogSummitApi.Infrastructure.Persistance.Repositories;

namespace LogSummitApi.Infrastructure.Factories;

public class RepositoryFactory : IRepositoryFactory
{
    private readonly LogSummitContext _context;
    private readonly IHttpClientFactory _httpFactory;

    public RepositoryFactory(LogSummitContext context, IHttpClientFactory httpFactory)
    {
        _context = context;
        _httpFactory = httpFactory;
    }

    public IHttpCountryRepository CreateHttpCountryRepository()
    {
        return new HttpCountryRepository(_httpFactory);
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
