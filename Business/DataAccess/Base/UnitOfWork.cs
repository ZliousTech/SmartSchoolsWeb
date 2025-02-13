using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Base
{
    public class UnitOfWork
    {
        public UnitOfWork(DbContext dbContext)
        {
            _context = dbContext;
            _context.Configuration.LazyLoadingEnabled = false;

        }
        DbContext _context;
        protected DbContext Context
        {
            get
            {
                return _context;
            }
        }
        public virtual IRepository<T> CreateRepository<T>() where T : class
        {
            RepositoryBase<T> repositoryBase = new RepositoryBase<T>((_context));
            return repositoryBase;
        }
        public virtual int Save()
        {
            return Context.SaveChanges();
        }
        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                // If disposing equals true, dispose all managed 
                // and unmanaged resources.
                if (disposing)
                {
                    Context.Dispose();
                }
            }
            disposed = true;
        }
        public void Detach(object entity)
        {
            //Context.Detach(entity);
        }
        public T CreateRepository<T, E>()
            where T : IRepository<E>
            where E : class
        {
            var d1 = typeof(T);
            object o = Activator.CreateInstance(d1, Context);
            return (T)o;
        }
    }

}
