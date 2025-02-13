using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class TimetablesRepository : RepositoryBase<Timetable>, ITimetablesRepository
    {
        public TimetablesRepository(DbContext context)
            : base(context)
        {

        }
    }

}
