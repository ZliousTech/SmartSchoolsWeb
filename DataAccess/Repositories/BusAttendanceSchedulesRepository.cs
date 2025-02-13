using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class BusAttendanceSchedulesRepository : RepositoryBase<BusAttendanceSchedule>, IBusAttendanceSchedulesRepository
    {
        public BusAttendanceSchedulesRepository(DbContext context)
            : base(context)
        {

        }
    }

}
