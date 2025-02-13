using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class OccasionTypesRepository : RepositoryBase<OccasionType>, IOccasionTypesRepository
    {
        public OccasionTypesRepository(DbContext context)
            : base(context)
        {

        }
    }

}
