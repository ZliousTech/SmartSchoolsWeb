using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class DisciplineActionsRepository : RepositoryBase<DisciplineAction>, IDisciplineActionsRepository
    {
        public DisciplineActionsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
