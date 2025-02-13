using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class ActiveInstalltionsRepository : RepositoryBase<ActiveInstalltion>, IActiveInstalltionsRepository
    {
        public ActiveInstalltionsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
