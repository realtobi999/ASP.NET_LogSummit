using LogSummitApi.Domain.Core.Entities;

namespace LogSummitApi.Domain.Core.Interfaces.Repositories;

public interface ISummitPushRepository : IBaseRepository<SummitPush>
{
    Task<SummitPush?> GetAsync(Guid id);
}
