using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class FeeTypesRepository : RepositoryBase<FeeType>, IFeeTypesRepository
    {
        public FeeTypesRepository(DbContext context)
            : base(context)
        {

        }
    }

}
