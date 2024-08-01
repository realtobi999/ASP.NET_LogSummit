using System.Linq.Expressions;

namespace LogSummitApi.Domain.Core.Interfaces.Repositories;

public interface IBaseRepository<T>
{
    IQueryable<T> Index();
    Task<T?> Get(Expression<Func<T, bool>> expression);
    void Create(T entity);
    void Delete(T entity);
}
