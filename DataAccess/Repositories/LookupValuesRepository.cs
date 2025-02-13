using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class LookupValuesRepository : RepositoryBase<LookupValue>, ILookupValuesRepository
    {
        public LookupValuesRepository(DbContext context)
            : base(context)
        {

        }
    }

}
