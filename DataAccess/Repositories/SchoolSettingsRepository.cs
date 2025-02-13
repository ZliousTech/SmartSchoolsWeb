using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class SchoolSettingsRepository : RepositoryBase<SchoolSetting>, ISchoolSettingsRepository
    {
        public SchoolSettingsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
