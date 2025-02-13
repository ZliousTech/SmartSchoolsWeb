using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class FeesRepository : RepositoryBase<Fee>, IFeesRepository
    {
        public FeesRepository(DbContext context)
            : base(context)
        {

        }
    }

}
