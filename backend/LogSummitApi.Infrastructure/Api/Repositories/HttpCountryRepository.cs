using System.Net.Http.Json;
using LogSummitApi.Domain.Core.Dto.Summit;
using LogSummitApi.Domain.Core.Exceptions.Http;
using LogSummitApi.Domain.Core.Interfaces.Repositories.Http;
using Microsoft.Extensions.Caching.Memory;

namespace LogSummitApi.Infrastructure.Api.Repositories;


public class HttpCountryRepository : IHttpCountryRepository
{
    private readonly HttpClient _client;
    private readonly IMemoryCache _cache;
    private const string CacheKey = "CountriesList";

    public HttpCountryRepository(IHttpClientFactory httpFactory, IMemoryCache cache)
    {
        _client = httpFactory.CreateClient();
        _cache = cache;
    }

    /// <summary>
    /// Retrieves a list of <c>Country</c> objects either from the cache or by making an API call if the data is not cached.
    /// <para>
    /// If the data is available in the cache, it will be returned directly. If not, the method will fetch the data from the API,
    /// cache it for future requests, and then return the data.
    /// </para>
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation. The task result is an <c>IEnumerable{Country}</c> containing the list of countries.
    /// </returns>
    /// <exception cref="ServiceUnavailable503Exception">
    /// Thrown when the API call fails or returns null data, indicating that the service is unavailable.
    /// </exception>

    public async Task<IEnumerable<Country>> IndexAsync()
    {
        var value = await _cache.GetOrCreateAsync(CacheKey, async options =>
        {
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
            options.SlidingExpiration = TimeSpan.FromMinutes(30);

            var countries = await _client.GetFromJsonAsync<List<Country>>("https://restcountries.com/v3.1/all");

            if (countries == null) throw new ServiceUnavailable503Exception("Failed to retrieve country data from the 'rest-country' API.");

            return countries;
        });

        return value!; // we already null-checked inside the cache method
    }
}