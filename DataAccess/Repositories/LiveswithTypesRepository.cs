using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class LiveswithTypesRepository : RepositoryBase<LiveswithType>, ILiveswithTypesRepository
    {
        public LiveswithTypesRepository(DbContext context)
            : base(context)
        {

        }
    }

}
