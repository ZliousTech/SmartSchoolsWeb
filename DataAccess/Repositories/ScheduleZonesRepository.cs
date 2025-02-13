using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class ScheduleZonesRepository : RepositoryBase<ScheduleZone>, IScheduleZonesRepository
    {
        public ScheduleZonesRepository(DbContext context)
            : base(context)
        {

        }
    }

}
