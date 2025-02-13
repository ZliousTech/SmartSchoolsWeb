using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class MaintenanceTypesRepository : RepositoryBase<MaintenanceType>, IMaintenanceTypesRepository
    {
        public MaintenanceTypesRepository(DbContext context)
            : base(context)
        {

        }
    }

}
