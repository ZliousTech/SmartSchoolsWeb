using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class AcademicCalendarsRepository : RepositoryBase<AcademicCalendar>, IAcademicCalendarsRepository
    {
        public AcademicCalendarsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
