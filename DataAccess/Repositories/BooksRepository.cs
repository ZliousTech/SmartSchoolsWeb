using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class BooksRepository : RepositoryBase<Book>, IBooksRepository
    {
        public BooksRepository(DbContext context)
            : base(context)
        {

        }
    }

}
