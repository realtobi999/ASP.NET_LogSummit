using System.Linq.Expressions;
using LogSummitApi.Domain.Core.Interfaces.Repositories;

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

    public IQueryable<T> Get(Expression<Func<T, bool>> expression)
    {
        return _context.Set<T>().Where(expression);
    }

    public IQueryable<T> Index()
    {
        return _context.Set<T>();
    }
}
