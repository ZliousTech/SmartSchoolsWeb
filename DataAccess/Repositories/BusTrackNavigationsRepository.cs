using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class BusTrackNavigationsRepository : RepositoryBase<BusTrackNavigation>, IBusTrackNavigationsRepository
    {
        public BusTrackNavigationsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
