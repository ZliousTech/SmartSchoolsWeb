using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class ScheduledBusTripsRepository : RepositoryBase<ScheduledBusTrip>, IScheduledBusTripsRepository
    {
        public ScheduledBusTripsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
