using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace DataAccess.Base
{
    public interface IRepository<T> : IDisposable where T : class
    {
        void Add(IEnumerable<T> entities);
        void Add(T entity);
        IQueryable<T> AsQueryable();
        void Delete(T entity);
        IEnumerable<T> Find(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes);
        T First(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes);
        IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includes);
        IEnumerable<T> Paging(int? page, int pageSize, bool ascending = true, Expression<Func<T, object>> orderby = null, params Expression<Func<T, object>>[] includes);
        IEnumerable<T> Paging(Expression<Func<T, bool>> where, int? page, int pageSize, bool ascending = true, Expression<Func<T, object>> orderby = null, params Expression<Func<T, object>>[] includes);
        IEnumerable<T> Paging(Expression<Func<T, bool>> where, int pageIndex, int pageSize, bool ascending, Expression<Func<T, object>> orderby, out int count, params Expression<Func<T, object>>[] includes);
        IEnumerable<T> Paging<TOrderby>(int? page, int pageSize, bool ascending, Expression<Func<T, TOrderby>> orderby, params Expression<Func<T, object>>[] includes);
        IEnumerable<T> Paging<TOrderby>(Expression<Func<T, bool>> where, int? page, int pageSize, bool ascending, Expression<Func<T, TOrderby>> orderby, params Expression<Func<T, object>>[] includes);
        IEnumerable<T> Paging<TOrderby>(Expression<Func<T, bool>> where, int pageIndex, int pageSize, bool ascending, Expression<Func<T, TOrderby>> orderby, out int count, params Expression<Func<T, object>>[] includes);
        T Single(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes);
        void Update(T entity);
        void UpdateOnlyProperty(T entity, string propertyName);
    }

}
