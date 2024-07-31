using LogSummitApi.Domain.Core.Interfaces.Factories;
using LogSummitApi.Domain.Core.Interfaces.Repositories;
using LogSummitApi.Infrastructure.Persistance;
using LogSummitApi.Infrastructure.Persistance.Repositories;

namespace LogSummitApi.Infrastructure.Factories;

public class RepositoryFactory : IRepositoryFactory
{
    private readonly LogSummitContext _context;

    public RepositoryFactory(LogSummitContext context)
    {
        _context = context;
    }

    public IUserRepository CreateUserRepository()
    {
        return new UserRepository(_context);
    }
}
