using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class SchoolBranchsRepository : RepositoryBase<SchoolBranch>, ISchoolBranchsRepository
    {
        public SchoolBranchsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
