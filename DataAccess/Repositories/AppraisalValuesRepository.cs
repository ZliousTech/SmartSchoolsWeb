using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class AppraisalValuesRepository : RepositoryBase<AppraisalValue>, IAppraisalValuesRepository
    {
        public AppraisalValuesRepository(DbContext context)
            : base(context)
        {

        }
    }

}
