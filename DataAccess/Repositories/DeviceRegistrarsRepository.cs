using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class DeviceRegistrarsRepository : RepositoryBase<DeviceRegistrar>, IDeviceRegistrarsRepository
    {
        public DeviceRegistrarsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
