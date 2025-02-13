using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class DiscountsRepository : RepositoryBase<Discount>, IDiscountsRepository
    {
        public DiscountsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
