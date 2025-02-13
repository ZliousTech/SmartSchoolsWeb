using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class BusShiftRotationSchedulesRepository : RepositoryBase<BusShiftRotationSchedule>, IBusShiftRotationSchedulesRepository
    {
        public BusShiftRotationSchedulesRepository(DbContext context)
            : base(context)
        {

        }
    }

}
