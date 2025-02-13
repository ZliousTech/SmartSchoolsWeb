using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class PaymentMethodsRepository : RepositoryBase<PaymentMethod>, IPaymentMethodsRepository
    {
        public PaymentMethodsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
