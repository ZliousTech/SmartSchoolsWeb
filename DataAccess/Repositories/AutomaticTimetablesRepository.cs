using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class AutomaticTimetablesRepository : RepositoryBase<AutomaticTimetable>, IAutomaticTimetablesRepository
    {
        public AutomaticTimetablesRepository(DbContext context)
            : base(context)
        {

        }
    }

}
