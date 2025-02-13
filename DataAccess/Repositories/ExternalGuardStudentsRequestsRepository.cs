using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class ExternalGuardStudentsRequestsRepository : RepositoryBase<ExternalGuardStudentsRequest>, IExternalGuardStudentsRequestsRepository
    {
        public ExternalGuardStudentsRequestsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
