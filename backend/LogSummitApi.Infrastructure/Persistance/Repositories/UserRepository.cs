using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Interfaces.Repositories;

namespace LogSummitApi.Infrastructure.Persistance.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(LogSummitContext context) : base(context)
    {
    }

    public async Task<User?> GetAsync(Guid id)
    {
        return await this.GetAsync(u => u.Id == id);
    }
}
