using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class TransportCategoryTypesRepository : RepositoryBase<TransportCategoryType>, ITransportCategoryTypesRepository
    {
        public TransportCategoryTypesRepository(DbContext context)
            : base(context)
        {

        }
    }

}
