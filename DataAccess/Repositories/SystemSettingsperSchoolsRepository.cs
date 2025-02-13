using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class SystemSettingsperSchoolsRepository : RepositoryBase<SystemSettingsperSchool>, ISystemSettingsperSchoolsRepository
    {
        public SystemSettingsperSchoolsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
