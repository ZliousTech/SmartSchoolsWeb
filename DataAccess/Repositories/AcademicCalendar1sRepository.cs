using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class AcademicCalendar1sRepository : RepositoryBase<AcademicCalendar1>, IAcademicCalendar1sRepository
    {
        public AcademicCalendar1sRepository(DbContext context)
            : base(context)
        {

        }
    }

}
