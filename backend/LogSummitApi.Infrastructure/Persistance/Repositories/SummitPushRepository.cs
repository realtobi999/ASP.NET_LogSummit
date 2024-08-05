using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LogSummitApi.Infrastructure.Persistance.Repositories;

public class SummitPushRepository : BaseRepository<SummitPush>, ISummitPushRepository
{
    public SummitPushRepository(LogSummitContext context) : base(context)
    {
    }

    public async Task<SummitPush?> GetAsync(Guid id)
    {
        return await _context.Set<SummitPush>().FirstOrDefaultAsync(sp => sp.Id == id);
    }
}
