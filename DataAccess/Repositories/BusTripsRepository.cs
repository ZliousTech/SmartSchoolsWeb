using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class BusTripsRepository : RepositoryBase<BusTrip>, IBusTripsRepository
    {
        public BusTripsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
