using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class AccountsDetailsRepository : RepositoryBase<AccountsDetail>, IAccountsDetailsRepository
    {
        public AccountsDetailsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
