using System.Linq.Expressions;

namespace LogSummitApi.Domain.Core.Interfaces.Repositories;

public interface IBaseRepository<T>
{
    Task<IEnumerable<T>> Index();
    Task<T?> Get(Expression<Func<T, bool>> expression);
    void Update(T entity);
    void Create(T entity);
    void Delete(T entity);
}
