using LogSummitApi.Domain.Core.Entities;
using LogSummitApi.Domain.Core.Interfaces.Repositories;

namespace LogSummitApi.Infrastructure.Persistance.Repositories;

public class UserRepository : BaseRepository<User>, IUserRepository
{
    public UserRepository(LogSummitContext context) : base(context)
    {
    }
}
