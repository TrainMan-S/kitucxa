using System;
using System.Linq;
using System.Linq.Expressions;

namespace WebApp.Data.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        T Add(T entity);

        T Update(T entity);

        T Delete(T entity);

        IQueryable<T> DeleteMulti(Expression<Func<T, bool>> predicate);

        T GetSingleById(int id);

        T GetSingleById(string id);

        T GetSingleByCondition(Expression<Func<T, bool>> expression, string[] includes = null);

        IQueryable<T> GetAll(string[] includes = null);

        IQueryable<T> GetMulti(Expression<Func<T, bool>> predicate, string[] includes = null);

        IQueryable<T> GetMultiPaging(Expression<Func<T, bool>> filter, out int total, int index = 0, int size = 12, string[] includes = null);

        int Count(Expression<Func<T, bool>> predicate);

        bool CheckContains(Expression<Func<T, bool>> predicate);

    }
}
