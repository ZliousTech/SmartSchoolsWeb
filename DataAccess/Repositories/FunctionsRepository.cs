using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class FunctionsRepository : RepositoryBase<Function>, IFunctionsRepository
    {
        public FunctionsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
