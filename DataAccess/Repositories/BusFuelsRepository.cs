using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class BusFuelsRepository : RepositoryBase<BusFuel>, IBusFuelsRepository
    {
        public BusFuelsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
