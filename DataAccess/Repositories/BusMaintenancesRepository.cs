using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class BusMaintenancesRepository : RepositoryBase<BusMaintenance>, IBusMaintenancesRepository
    {
        public BusMaintenancesRepository(DbContext context)
            : base(context)
        {

        }
    }

}
