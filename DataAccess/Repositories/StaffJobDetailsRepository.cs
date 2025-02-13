using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class StaffJobDetailsRepository : RepositoryBase<StaffJobDetail>, IStaffJobDetailsRepository
    {
        public StaffJobDetailsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
