using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class AttendanceTypesRepository : RepositoryBase<AttendanceType>, IAttendanceTypesRepository
    {
        public AttendanceTypesRepository(DbContext context)
            : base(context)
        {

        }
    }

}
