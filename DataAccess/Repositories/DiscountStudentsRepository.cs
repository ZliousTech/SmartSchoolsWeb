using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class DiscountStudentsRepository : RepositoryBase<DiscountStudent>, IDiscountStudentsRepository
    {
        public DiscountStudentsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
