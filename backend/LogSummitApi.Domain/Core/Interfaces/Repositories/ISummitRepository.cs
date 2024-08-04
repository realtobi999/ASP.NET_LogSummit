using LogSummitApi.Domain.Core.Entities;

namespace LogSummitApi.Domain.Core.Interfaces.Repositories;

public interface ISummitRepository : IBaseRepository<Summit>
{
    Task<Summit?> Get(Guid id);
}
