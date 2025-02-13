using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class MaritalStatussRepository : RepositoryBase<MaritalStatus>, IMaritalStatussRepository
    {
        public MaritalStatussRepository(DbContext context)
            : base(context)
        {

        }
    }

}
