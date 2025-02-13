using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Base
{
    public class RepositoryBase<T> : IRepository<T> where T : class
    {
        DbContext _context;
        DbSet<T> _objectSet;
        public DbContext Context
        {
            get
            {
                return _context;
            }
        }
        public DbSet<T> ObjectSet
        {
            get
            {
                return _objectSet ?? (_objectSet = this.Context.Set<T>());

            }
            private set { _objectSet = value; }
        }
        public RepositoryBase(DbContext context)
        {
            _context = context;
        }

        #region Public Methods
        public virtual void Add(T entity)
        {
            this.ObjectSet.Add(entity);
        }
        public virtual void Delete(T entity)
        {
            try
            {
                ObjectSet.Attach(entity);
            }
            catch
            {
            }
            ObjectSet.Remove(entity);
        }
        public virtual IEnumerable<T> Find(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes)
        {
            IQueryable<T> queryable;
            queryable = GetQueryObject(includes).Where<T>(where);
            return queryable;
        }
        public virtual IEnumerable<T> GetAll(params Expression<Func<T, object>>[] includes)
        {
            List<T> list;
            list = GetQueryObject(includes).ToList();
            return list;
        }
        public virtual void Update(T entity)
        {
            try
            {
                Context.Entry(entity).State = EntityState.Modified;
            }
            catch
            {
            }
        }
        public virtual void UpdateOnlyProperty(T entity, string propertyName)
        {
            try
            {
                ObjectSet.Attach(entity);
                Context.Entry(entity).Property(propertyName).IsModified = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public virtual T Single(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes)
        {
            T result;

            if (includes.Count() > 0)
            {
                DbQuery<T> query = GetQueryObject(includes);

                result = query.SingleOrDefault(where);
            }
            else
            {
                result = this.ObjectSet.SingleOrDefault(where);
            }

            return result;
        }
        public virtual T First(Expression<Func<T, bool>> where, params Expression<Func<T, object>>[] includes)
        {
            T result;
            DbQuery<T> query = GetQueryObject(includes);
            result = query.FirstOrDefault(where);
            return result;
        }
        public virtual IQueryable<T> AsQueryable()
        {
            return ObjectSet;
        }
        public virtual void Dispose()
        {
        }
        #endregion

        #region Private Methods
        protected DbQuery<T> GetQueryObject(Expression<Func<T, object>>[] includes)
        {
            string path;
            DbQuery<T> query = ObjectSet as DbQuery<T>;
            if (includes.Count() > 0)
            {
                foreach (Expression<Func<T, object>> expression in includes)
                {
                    path = GetPath(expression);
                    query = query.Include(path);
                }
            }
            return query;
        }
        protected DbQuery<T> GetQueryObject(Expression<Func<T, bool>> where, Expression<Func<T, object>>[] includes)
        {
            string path;
            DbQuery<T> query = ObjectSet.AsNoTracking().Where(where) as DbQuery<T>;
            foreach (Expression<Func<T, object>> expression in includes)
            {
                path = GetPath(expression);
                query = query.Include(path);
            }
            return query;
        }
        private string GetPath(Expression exp)
        {
            switch (exp.NodeType)
            {
                case ExpressionType.MemberAccess:
                    var name = GetPath(((MemberExpression)exp).Expression) ?? "";

                    if (name.Length > 0)
                        name += ".";

                    return name + ((MemberExpression)exp).Member.Name;
                case ExpressionType.Call:
                    string CompositeName = "";
                    foreach (var arg in ((MethodCallExpression)exp).Arguments)
                    {
                        CompositeName += GetPath(arg);
                        if (CompositeName.Length > 0)
                            CompositeName += ".";
                    }
                    return CompositeName.TrimEnd(char.Parse("."));

                case ExpressionType.Convert:
                case ExpressionType.Quote:
                    return GetPath(((UnaryExpression)exp).Operand);

                case ExpressionType.Lambda:
                    return GetPath(((LambdaExpression)exp).Body);

                default:
                    return null;
            }
        }
        #endregion

        protected bool IsAttachedTo(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            return (Context.Entry(entity).State != EntityState.Detached);
        }

        #region Paging
        public virtual IEnumerable<T> Paging<TOrderby>(int? page, int pageSize, bool ascending, Expression<Func<T, TOrderby>> orderby, params Expression<Func<T, object>>[] includes)
        {
            IEnumerable<T> result;
            DbQuery<T> query = GetQueryObject(includes);
            if (ascending)
            {
                result = query.OrderBy(orderby).Skip((page ?? 0) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                result = query.OrderByDescending(orderby).Skip((page ?? 0) * pageSize).Take(pageSize).ToList();
            }
            return result;
        }
        public virtual IEnumerable<T> Paging<TOrderby>(Expression<Func<T, bool>> where, int? page, int pageSize, bool ascending, Expression<Func<T, TOrderby>> orderby, params Expression<Func<T, object>>[] includes)
        {
            IEnumerable<T> result;
            DbQuery<T> query = GetQueryObject(includes);
            if (ascending)
            {
                result = query.Where(where).OrderBy(orderby).Skip((page ?? 0) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                result = query.Where(where).OrderByDescending(orderby).Skip((page ?? 0) * pageSize).Take(pageSize).ToList();
            }
            return result;
        }
        public virtual IEnumerable<T> Paging(int? page, int pageSize, bool ascending = true, Expression<Func<T, object>> orderby = null, params Expression<Func<T, object>>[] includes)
        {
            IEnumerable<T> result;
            DbQuery<T> query = GetQueryObject(includes);
            if (orderby != null)
            {
                result = query.Skip((page ?? 0) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                result = query.OrderBy(orderby).Cast<T>().Skip((page ?? 0) * pageSize).Take(pageSize).ToList();
            }
            return result;
        }
        public virtual IEnumerable<T> Paging(Expression<Func<T, bool>> where, int? page, int pageSize, bool ascending = true, Expression<Func<T, object>> orderby = null, params Expression<Func<T, object>>[] includes)
        {
            IEnumerable<T> result = null;
            DbQuery<T> query = GetQueryObject(where, includes);

            if (orderby == null)
            {
                result = query.Skip((page ?? 0) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                if (ascending)
                    result = query.OrderBy(orderby).Cast<T>().Skip((page ?? 0) * pageSize).Take(pageSize).ToList();
                else
                    result = query.OrderByDescending(orderby).Cast<T>().Skip((page ?? 0) * pageSize).Take(pageSize).ToList();
            }
            return result;
        }

        /// <summary>
        /// When implemented returns a query with the specified parameters.
        /// </summary>
        /// <param name="where">The where conditions.</param>
        /// <param name="pageIndex">The page index.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="ascending">The order direction.</param>
        /// <param name="orderby">The order key.</param>
        /// <param name="count">The count of the records returned in the result.</param>
        /// <param name="includes">The navigation properties to include with the returned list.</param>
        /// <returns>A query containing the specified parameters</returns>
        public IEnumerable<T> Paging<TOrderby>(Expression<Func<T, bool>> where, int pageIndex, int pageSize, bool ascending, Expression<Func<T, TOrderby>> orderby, out int count, params Expression<Func<T, object>>[] includes)
        {
            DbQuery<T> query = GetQueryObject(where, includes);
            count = query.Count();
            if (ascending)
            {
                return query.OrderBy(orderby).Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }
            else
            {
                return query.OrderByDescending(orderby).Skip(pageIndex * pageSize).Take(pageSize).ToList();
            }
        }
        public IEnumerable<T> Paging(Expression<Func<T, bool>> @where, int pageIndex, int pageSize, bool @ascending, Expression<Func<T, object>> @orderby, out int count, params Expression<Func<T, object>>[] includes)
        {
            DbQuery<T> query = GetQueryObject(where, includes);
            count = query.Count();
            // return @ascending ? ((ObjectQuery<T>)query.OrderBy(@orderby)).Skip(pageIndex * pageSize).Take(pageSize).ToList() : (query.OrderByDescending(@orderby)).Skip(pageIndex * pageSize).Take(pageSize).ToList();
            // Fix ObjectQuery casting Problem (WorkAround) && Fix Paging Equation :: MShaabani  //TODO :: Refelect Equation on all methods here!
            /*
                Microsoft Explain:
                With previous version of Entity Framework a model created with the EF Designer would generate a 
                context that derived from ObjectContext and entity classes that derived from EntityObject.
                Starting with EF4.1 we recommended swapping to a code generation template that generates a 
                context deriving from DbContext and POCO entity classes. 
            */
            if (ascending)
            {
                return ((DbQuery<T>)query.OrderBy(@orderby)).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
            }
            else
            {
                return ((DbQuery<T>)query.OrderByDescending(@orderby)).Skip(pageSize * (pageIndex - 1)).Take(pageSize).ToList();
            }
        }
        public virtual void Add(IEnumerable<T> entities)
        {
            this.ObjectSet.AddRange(entities);
        }
        #endregion
    }

}
