using System.Linq.Expressions;

namespace InStockWebAppBLL.Features.Interfaces;

public interface IGenericRepository<T> where T : class
{
    Task<T?> GetById(int id);

    Task<IEnumerable<T>> GetAll(Expression<Func<T, bool>>? filter = default,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = default,
        params Expression<Func<T, object>>[] includes);

    Task<T> GetFirstOrDefault(Expression<Func<T, bool>>? filter = default,
        params Expression<Func<T, object>>[] includes);

    IQueryable<T> Query(Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = default);

    Task Add(T entity);
    void Delete(T entity);
    void Update(T entity);
}