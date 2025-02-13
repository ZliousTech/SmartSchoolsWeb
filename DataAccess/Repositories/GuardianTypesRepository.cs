using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class GuardianTypesRepository : RepositoryBase<GuardianType>, IGuardianTypesRepository
    {
        public GuardianTypesRepository(DbContext context)
            : base(context)
        {

        }
    }

}
