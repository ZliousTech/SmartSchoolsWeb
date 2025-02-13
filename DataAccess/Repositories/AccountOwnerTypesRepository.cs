using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class AccountOwnerTypesRepository : RepositoryBase<AccountOwnerType>, IAccountOwnerTypesRepository
    {
        public AccountOwnerTypesRepository(DbContext context)
            : base(context)
        {

        }
    }

}
