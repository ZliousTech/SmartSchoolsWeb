using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class ActiveProductKeysRepository : RepositoryBase<ActiveProductKey>, IActiveProductKeysRepository
    {
        public ActiveProductKeysRepository(DbContext context)
            : base(context)
        {

        }
    }

}
