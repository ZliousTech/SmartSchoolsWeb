using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class BusInfosRepository : RepositoryBase<BusInfo>, IBusInfosRepository
    {
        public BusInfosRepository(DbContext context)
            : base(context)
        {

        }
    }

}
