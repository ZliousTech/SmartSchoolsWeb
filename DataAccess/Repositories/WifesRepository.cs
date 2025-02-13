using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class WifesRepository : RepositoryBase<Wife>, IWifesRepository
    {
        public WifesRepository(DbContext context)
            : base(context)
        {

        }
    }

}
