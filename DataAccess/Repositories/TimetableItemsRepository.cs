using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class TimetableItemsRepository : RepositoryBase<TimetableItem>, ITimetableItemsRepository
    {
        public TimetableItemsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
