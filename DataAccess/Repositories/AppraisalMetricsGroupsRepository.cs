using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class AppraisalMetricsGroupsRepository : RepositoryBase<AppraisalMetricsGroup>, IAppraisalMetricsGroupsRepository
    {
        public AppraisalMetricsGroupsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
