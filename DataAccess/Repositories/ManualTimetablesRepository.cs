using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class ManualTimetablesRepository : RepositoryBase<ManualTimetable>, IManualTimetablesRepository
    {
        public ManualTimetablesRepository(DbContext context)
            : base(context)
        {

        }
    }

}
