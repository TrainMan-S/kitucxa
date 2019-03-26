using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace WebApp.Data.Infrastructure
{
    public abstract class RepositoryBase<T> : IRepository<T> where T : class
    {
        private WebAppDbContext dataContext;
        private readonly IDbSet<T> dbSet;

        protected IDbFactory DbFactory
        {
            get; private set;
        }

        protected WebAppDbContext DbContext
        {
            get { return dataContext ?? (dataContext = DbFactory.Init()); }
        }

        protected RepositoryBase(IDbFactory dbFactory)
        {
            DbFactory = dbFactory;
            dbSet = DbContext.Set<T>();
        }

        // ----- Implement -----
        public virtual T Add(T entity)
        {
            return dbSet.Add(entity);
        }

        public virtual T Update(T entity)
        {
            dbSet.Attach(entity);
            dataContext.Entry(entity).State = EntityState.Modified;
            return entity;
        }

        public virtual T Delete(T entity)
        {
            return dbSet.Remove(entity);
        }

        public virtual IQueryable<T> DeleteMulti(Expression<Func<T, bool>> predicate)
        {
            IQueryable<T> objects = dbSet.Where<T>(predicate).AsQueryable();
            foreach (T item in objects)
            {
                dbSet.Remove(item);
            }
            return objects;
        }

        public virtual T GetSingleById(int id)
        {
            return dbSet.Find(id);
        }

        public virtual T GetSingleById(string id)
        {
            return dbSet.Find(id);
        }

        public virtual T GetSingleByCondition(Expression<Func<T, bool>> predicate, string[] includes = null)
        {
            return GetAll(includes).FirstOrDefault(predicate);
        }

        public virtual IQueryable<T> GetAll(string[] includes = null)
        {
            if (includes != null && includes.Any())
            {
                var query = dataContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                {
                    query = query.Include(include);
                }
                return query.AsQueryable();
            }
            else
            {
                return dataContext.Set<T>().AsQueryable();
            }
        }

        public virtual IQueryable<T> GetMulti(Expression<Func<T, bool>> predicate, string[] includes = null)
        {
            if (includes != null && includes.Count() > 0)
            {
                var query = dataContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                {
                    query = query.Include(include);
                }
                return query.Where<T>(predicate).AsQueryable<T>();
            }
            else
            {
                return dataContext.Set<T>().Where<T>(predicate).AsQueryable<T>();
            }
        }

        public virtual IQueryable<T> GetMultiPaging(Expression<Func<T, bool>> predicate, out int total, int index = 0, int size = 12, string[] includes = null)
        {
            int skipCount = index * size;
            IQueryable<T> _resetSet;

            if (includes != null && includes.Count() > 0)
            {
                var query = dataContext.Set<T>().Include(includes.First());
                foreach (var include in includes.Skip(1))
                {
                    query = query.Include(include);
                }

                _resetSet = predicate != null ? query.Where<T>(predicate).AsQueryable() : query.AsQueryable();
            }
            else
            {
                _resetSet = predicate != null ? dataContext.Set<T>().Where<T>(predicate).AsQueryable() : dataContext.Set<T>().AsQueryable();
            }

            _resetSet = skipCount == 0 ? _resetSet.Take(size) : _resetSet.Skip(skipCount).Take(size);
            total = _resetSet.Count();

            return _resetSet.AsQueryable();
        }

        public virtual int Count(Expression<Func<T, bool>> predicate)
        {
            return dbSet.Count<T>(predicate);
        }

        public virtual bool CheckContains(Expression<Func<T, bool>> predicate)
        {
            return dataContext.Set<T>().Any<T>(predicate);
        }

    }
}
