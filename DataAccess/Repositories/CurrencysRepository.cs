using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class CurrencysRepository : RepositoryBase<Currency>, ICurrencysRepository
    {
        public CurrencysRepository(DbContext context)
            : base(context)
        {

        }
    }

}
