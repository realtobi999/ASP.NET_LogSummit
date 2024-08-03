using System.Linq.Expressions;
using LogSummitApi.Domain.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace LogSummitApi.Infrastructure.Persistance.Repositories;

public abstract class BaseRepository<T> : IBaseRepository<T> where T : class
{
    protected readonly LogSummitContext _context;

    public BaseRepository(LogSummitContext context)
    {
        _context = context;
    }

    public void Create(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public void Delete(T entity)
    {
        _context.Set<T>().Add(entity);
    }

    public Task<T?> Get(Expression<Func<T, bool>> expression)
    {
        return _context.Set<T>().FirstOrDefaultAsync(expression);
    }

    public async Task<IEnumerable<T>> Index()
    {
        return await _context.Set<T>().ToListAsync();
    }
}
