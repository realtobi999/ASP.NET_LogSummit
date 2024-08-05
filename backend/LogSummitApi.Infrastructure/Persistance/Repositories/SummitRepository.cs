using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LogSummitApi.Infrastructure.Persistance.Repositories;

public class SummitRepository : BaseRepository<Summit>, ISummitRepository
{
    public SummitRepository(LogSummitContext context) : base(context)
    {
    }

    public async Task<Summit?> GetAsync(Guid id)
    {
        return await this.GetAsync(s => s.Id == id); 
    }
}
