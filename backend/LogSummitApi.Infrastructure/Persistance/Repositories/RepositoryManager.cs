using LogSummitApi.Domain.Core.Exceptions;
using LogSummitApi.Domain.Core.Interfaces.Factories;
using LogSummitApi.Domain.Core.Interfaces.Repositories;

namespace LogSummitApi.Infrastructure.Persistance.Repositories;

public class RepositoryManager : IRepositoryManager
{
    private readonly LogSummitContext _context;
    private readonly IRepositoryFactory _factory;
    private readonly Lazy<IUserRepository> _users;
    private readonly Lazy<ISummitRepository> _summits;

    public RepositoryManager(LogSummitContext context, IRepositoryFactory factory)
    {
        _context = context;
        _factory = factory;

        // lazy loading
        _users = new(() => _factory.CreateUserRepository());
        _summits = new(() => _factory.CreateSummitRepository());
    }

    public IUserRepository Users => _users.Value;

    public ISummitRepository Summit => _summits.Value;

    public async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }

    public async Task SaveSafelyAsync()
    {
        var affected = await this.SaveAsync();

        if (affected == 0)
            throw new ZeroRowsAffectedException();
    }
}
