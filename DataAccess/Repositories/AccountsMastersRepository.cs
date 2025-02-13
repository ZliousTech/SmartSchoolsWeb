using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class AccountsMastersRepository : RepositoryBase<AccountsMaster>, IAccountsMastersRepository
    {
        public AccountsMastersRepository(DbContext context)
            : base(context)
        {

        }
    }

}
