using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class SystemObjectsRepository : RepositoryBase<SystemObject>, ISystemObjectsRepository
    {
        public SystemObjectsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
