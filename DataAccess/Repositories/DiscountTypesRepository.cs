using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class DiscountTypesRepository : RepositoryBase<DiscountType>, IDiscountTypesRepository
    {
        public DiscountTypesRepository(DbContext context)
            : base(context)
        {

        }
    }

}
