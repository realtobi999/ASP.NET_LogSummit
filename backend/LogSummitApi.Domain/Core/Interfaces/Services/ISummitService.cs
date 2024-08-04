using LogSummitApi.Domain.Core.Dto.Summit;
using LogSummitApi.Domain.Core.Entities;

namespace LogSummitApi.Domain.Core.Interfaces.Services;

public interface ISummitService
{
    Task<IEnumerable<Summit>> Index();
    Task<Summit> Get(Guid id);
    Task<Summit> Create(CreateSummitDto createSummitDto);
    Task Update(Summit summit, UpdateSummitDto updateSummitDto);
    Task<IEnumerable<string>> GetValidCountries();
}
