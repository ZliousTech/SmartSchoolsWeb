using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class SessionsRepository : RepositoryBase<Session>, ISessionsRepository
    {
        public SessionsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
