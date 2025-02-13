using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class AppraisalsRepository : RepositoryBase<Appraisal>, IAppraisalsRepository
    {
        public AppraisalsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
