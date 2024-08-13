using LogSummitApi.Domain.Core.Interfaces.Factories;
using LogSummitApi.Domain.Core.Interfaces.Repositories.HTTP;
using LogSummitApi.Infrastructure.Api.Repositories;
using Microsoft.Extensions.Caching.Memory;

namespace LogSummitApi.Infrastructure.Factories;

public class HttpRepositoryFactory : IHttpRepositoryFactory
{
    private readonly IHttpClientFactory _httpFactory;
    private readonly IMemoryCache _cache;

    public HttpRepositoryFactory(IHttpClientFactory httpFactory, IMemoryCache cache)
    {
        _httpFactory = httpFactory;
        _cache = cache;
    }

    public IHttpCountryRepository CreateCountryHttpRepository()
    {
        return new HttpCountryRepository(_httpFactory, _cache);
    }
}
