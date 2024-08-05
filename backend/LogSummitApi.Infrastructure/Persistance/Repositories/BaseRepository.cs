﻿using System.Linq.Expressions;
using LogSummitApi.Domain.Core.Attributes;
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
        _context.Set<T>().Remove(entity);
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> expression)
    {
        return await GetQueryable().FirstOrDefaultAsync(expression);
    }

    public async Task<IEnumerable<T>> IndexAsync()
    {
        return await GetQueryable().ToListAsync();
    }

    public void Update(T entity)
    {
        _context.Set<T>().Update(entity);
    }

    private IQueryable<T> GetQueryable()
    {
        var query = _context.Set<T>().AsQueryable();
        return IncludeNavigationProperties(query);
    }

    private IQueryable<T> IncludeNavigationProperties(IQueryable<T> query)
    {
        var navigationProperties = typeof(T).GetProperties()
           .Where(p => p.GetCustomAttributes(typeof(IncludeInQueryingAttribute), false).Length != 0);

        foreach (var property in navigationProperties)
        {
            query = query.Include(property.Name);
        }

        return query;
    }

}
