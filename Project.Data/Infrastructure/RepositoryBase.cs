using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using PagedList;
using Project.Model.Models;

namespace Project.Data.Infrastructure
{
    public abstract class RepositoryBase<T> where T : class
    {
        private ApplicationDbContext dataContext;
        private readonly IDbSet<T> dbset;
        protected RepositoryBase(IDatabaseFactory databaseFactory)
        {
            DatabaseFactory = databaseFactory;
            dbset = DataContext.Set<T>();
        }

        protected IDatabaseFactory DatabaseFactory
        {
            get;
            private set;
        }

        protected ApplicationDbContext DataContext
        {
            get { return dataContext ?? (dataContext = DatabaseFactory.Get()); }
        }
        public virtual void Add(T entity)
        {
            dbset.Add(entity);
        }
        public virtual void Update(T entity)
        {
            dbset.Attach(entity);
            dataContext.Entry(entity).State = EntityState.Modified;
        }
        public virtual void Delete(T entity)
        {
            dbset.Remove(entity);
        }
        public virtual void Delete(Expression<Func<T, bool>> where)
        {
            IEnumerable<T> objects = dbset.Where<T>(where).AsEnumerable();
            foreach (T obj in objects)
                dbset.Remove(obj);
        }
        public virtual T GetById(long id)
        {
            return dbset.Find(id);
        }

        public virtual T GetById(string id)
        {
            return dbset.Find(id);
        }

        public virtual T GetById(Guid id)
        {
            return dbset.Find(id);
        }

        public virtual IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            var query = this.AppendIncludes(dbset, includes);

            return query.ToList();
        }

        public virtual IEnumerable<T> GetMany(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes)
        {
            var query = dbset.Where(where);
            query = this.AppendIncludes(query, includes);

            return query.ToList();
        }

        /// <summary>
        /// Return a paged list of entities
        /// </summary>
        /// <typeparam name="TOrder"></typeparam>
        /// <param name="page">Which page to retrieve</param>
        /// <param name="where">Where clause to apply</param>
        /// <param name="order">Order by to apply</param>
        /// <returns></returns>
        public virtual IPagedList<T> GetPage<TOrder>(Page page, Expression<Func<T, bool>> where, Expression<Func<T, TOrder>> order,
            params Expression<Func<T, object>>[] includes)
        {
            var query = dbset.OrderBy(order).Where(where).GetPage(page);
            query = this.AppendIncludes(query, includes);
            
            var results = query.ToList();
            var total = dbset.Count(where);

            return new StaticPagedList<T>(results, page.PageNumber, page.PageSize, total);
        }

        public T Get(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes)
        {
            var query = this.dbset.Where(where);
            query = this.AppendIncludes(query, includes);

            return query.FirstOrDefault<T>();
        }

        private IQueryable<T> AppendIncludes(IQueryable<T> query, IEnumerable<Expression<Func<T, object>>> includes)
        {
            foreach (var inc in includes)
            {
                query = query.Include(inc);
            }

            return query;
        }
    }
}
