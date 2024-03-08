using System.Linq.Expressions;
using InStockWebAppBLL.Features.Interfaces;
using InStockWebAppDAL.Context;
using Microsoft.EntityFrameworkCore;

namespace InStockWebAppBLL.Features.Repositories;

public abstract class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly ApplicationDbContext _applicationDbContext;

    protected GenericRepository(ApplicationDbContext applicationDbContext)
    {
        _applicationDbContext = applicationDbContext;
    }

    public async Task<T?> GetById(int id) =>
        await _applicationDbContext.Set<T>().FindAsync(id);

    public async Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = default,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = default,
        params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _applicationDbContext.Set<T>();

        query = includes.Aggregate(query, (current, include) => current.Include(include));

        if (filter is not null)
            query = query.Where(filter);
        if (orderBy is not null)
            query = orderBy(query);
        return await query.ToListAsync();
    }

    public async Task<T> GetFirstOrDefault(Expression<Func<T, bool>>? filter = default,
        params Expression<Func<T, object>>[] includes)
    {
        IQueryable<T> query = _applicationDbContext.Set<T>();
        query = includes.Aggregate(query, (current, include) => current.Include(include));
        return await query.FirstOrDefaultAsync(filter);
    }

    public IQueryable<T> Query(Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = default)
    {
        IQueryable<T> query = _applicationDbContext.Set<T>();
        if (filter is not null)
            query.Where(filter);
        if (orderBy is not null)
            query = orderBy(query);
        return query;
    }

    public abstract Task Add(T entity);

    public abstract void Delete(T entity);

    public abstract void Update(T entity);
}