using System.Net.Http.Json;
using LogSummitApi.Domain.Core.Dto.Summit;
using LogSummitApi.Domain.Core.Exceptions.Http;
using LogSummitApi.Domain.Core.Interfaces.Repositories.HTTP;
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
    /// Retrieves a list of <see cref="CountryDto"/> from the cache or fetches it from the API if not present in the cache.
    /// </summary>
    /// <returns>
    /// A task that represents the asynchronous operation containing <see cref="IEnumerable{CountryDto}"/> 
    /// representing the list of countries.
    /// </returns>
    /// <exception cref="ServiceUnavailable503Exception">
    /// Thrown when the API call fails and returns null data.
    /// </exception>
    public async Task<IEnumerable<CountryDto>> IndexAsync()
    {
        var value = await _cache.GetOrCreateAsync(CacheKey, async options =>
        {
            options.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
            options.SlidingExpiration = TimeSpan.FromMinutes(30);

            var countries = await _client.GetFromJsonAsync<List<CountryDto>>("https://restcountries.com/v3.1/all");

            if (countries == null) throw new ServiceUnavailable503Exception("Failed to retrieve country data from the 'rest-country' API.");

            return countries;
        });

        return value!; // we already null-checked inside the cache method
    }
}