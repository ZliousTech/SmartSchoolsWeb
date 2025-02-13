using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class BookUnitsRepository : RepositoryBase<BookUnit>, IBookUnitsRepository
    {
        public BookUnitsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
