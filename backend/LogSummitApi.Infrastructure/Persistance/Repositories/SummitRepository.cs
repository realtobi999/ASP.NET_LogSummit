using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Interfaces.Repositories;
using LogSummitApi.Infrastructure.Persistance;

namespace LogSummitApi.Infrastructure.Persistance.Repositories;

public class SummitRepository : BaseRepository<Summit>, ISummitRepository
{
    public SummitRepository(LogSummitContext context) : base(context)
    {
    }
}
