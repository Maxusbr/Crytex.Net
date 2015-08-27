using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using PagedList;

namespace Project.Data.Infrastructure
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
        void Delete(Expression<Func<T, bool>> where);
        T GetById(long id);
        T GetById(string id);
        T GetById(Guid id);
        T Get(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes);
        IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includes);
        IEnumerable<T> GetMany(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes);
        IPagedList<T> GetPage<TOrder>(Page page, Expression<Func<T, bool>> where, Expression<Func<T, TOrder>> order, params Expression<Func<T, object>>[] includes);
    }
}
