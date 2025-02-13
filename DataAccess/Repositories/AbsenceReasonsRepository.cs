using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class AbsenceReasonsRepository : RepositoryBase<AbsenceReason>, IAbsenceReasonsRepository
    {
        public AbsenceReasonsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
