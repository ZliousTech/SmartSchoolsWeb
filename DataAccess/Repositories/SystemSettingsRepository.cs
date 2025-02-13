using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class SystemSettingsRepository : RepositoryBase<SystemSetting>, ISystemSettingsRepository
    {
        public SystemSettingsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
