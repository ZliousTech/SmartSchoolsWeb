using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class AccountTypesRepository : RepositoryBase<AccountType>, IAccountTypesRepository
    {
        public AccountTypesRepository(DbContext context)
            : base(context)
        {

        }
    }

}
