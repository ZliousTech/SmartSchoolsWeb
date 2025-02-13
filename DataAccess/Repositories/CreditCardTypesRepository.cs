using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class CreditCardTypesRepository : RepositoryBase<CreditCardType>, ICreditCardTypesRepository
    {
        public CreditCardTypesRepository(DbContext context)
            : base(context)
        {

        }
    }

}
