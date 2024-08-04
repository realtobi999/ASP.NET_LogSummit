using System.Net.Http.Json;
using LogSummitApi.Domain.Core.Dto.Summit;
using LogSummitApi.Domain.Core.Exceptions.HTTP;
using LogSummitApi.Domain.Core.Interfaces.Repositories.HTTP;

namespace LogSummitApi.Infrastructure.HTTP.Repositories;

public class HttpCountryRepository : IHttpCountryRepository
{
    private readonly HttpClient _client;

    public HttpCountryRepository(IHttpClientFactory httpFactory)
    {
        _client = httpFactory.CreateClient();
    }

    public async Task<IEnumerable<CountryDto>> Index()
    {
        var countries = await _client.GetFromJsonAsync<List<CountryDto>>("https://restcountries.com/v3.1/all"); 

        if (countries is null) throw new ServiceUnavailable503Exception("Failed to retrieve country data from the 'rest-country' API.");
        
        return countries;
    }
}
