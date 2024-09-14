using LogSummitApi.Domain.Core.Entities;

namespace LogSummitApi.Domain.Core.Interfaces.Services;

public interface ISummitService
{
    Task<IEnumerable<Summit>> IndexAsync();
    Task<Summit> GetAsync(Guid id);
    Task CreateAsync(Summit summit);
    Task UpdateAsync(Summit summit);
    Task DeleteAsync(Summit summit);
    Task<IEnumerable<string>> GetValidCountriesAsync();
}
