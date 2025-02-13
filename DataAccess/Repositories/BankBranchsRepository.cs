using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class BankBranchsRepository : RepositoryBase<BankBranch>, IBankBranchsRepository
    {
        public BankBranchsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
