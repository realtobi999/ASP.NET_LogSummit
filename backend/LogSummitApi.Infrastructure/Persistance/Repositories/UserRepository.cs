using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LogSummitApi.Infrastructure.Persistance.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(LogSummitContext context) : base(context)
    {
    }

    public async Task<User?> Get(Guid id)
    {
        return await _context.Set<User>().FirstOrDefaultAsync(u => u.Id == id);
    }
}
