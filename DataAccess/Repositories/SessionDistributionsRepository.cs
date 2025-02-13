using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class SessionDistributionsRepository : RepositoryBase<SessionDistribution>, ISessionDistributionsRepository
    {
        public SessionDistributionsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
