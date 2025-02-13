using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class TimetableConditionsRepository : RepositoryBase<TimetableCondition>, ITimetableConditionsRepository
    {
        public TimetableConditionsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
