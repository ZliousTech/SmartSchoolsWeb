using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class TransportCategorysRepository : RepositoryBase<TransportCategory>, ITransportCategorysRepository
    {
        public TransportCategorysRepository(DbContext context)
            : base(context)
        {

        }
    }

}
