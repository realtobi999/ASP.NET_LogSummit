using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LogSummitApi.Infrastructure.Persistance.Repositories;

public class SummitRepository : BaseRepository<Summit>, ISummitRepository
{
    public SummitRepository(LogSummitContext context) : base(context)
    {
    }

    public async Task<Summit?> Get(Guid id)
    {
        return await _context.Set<Summit>().FirstOrDefaultAsync(s => s.Id == id);
    }
}
