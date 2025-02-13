using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class AttendancesRepository : RepositoryBase<Attendance>, IAttendancesRepository
    {
        public AttendancesRepository(DbContext context)
            : base(context)
        {

        }
    }

}
