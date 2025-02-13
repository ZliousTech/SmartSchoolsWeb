using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class BookChaptersRepository : RepositoryBase<BookChapter>, IBookChaptersRepository
    {
        public BookChaptersRepository(DbContext context)
            : base(context)
        {

        }
    }

}
