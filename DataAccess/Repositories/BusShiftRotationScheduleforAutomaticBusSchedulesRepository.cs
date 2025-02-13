using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class BusShiftRotationScheduleforAutomaticBusSchedulesRepository : RepositoryBase<BusShiftRotationScheduleforAutomaticBusSchedule>, IBusShiftRotationScheduleforAutomaticBusSchedulesRepository
    {
        public BusShiftRotationScheduleforAutomaticBusSchedulesRepository(DbContext context)
            : base(context)
        {

        }
    }

}
