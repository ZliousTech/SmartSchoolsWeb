using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class AppraisalTypesRepository : RepositoryBase<AppraisalType>, IAppraisalTypesRepository
    {
        public AppraisalTypesRepository(DbContext context)
            : base(context)
        {

        }
    }

}
