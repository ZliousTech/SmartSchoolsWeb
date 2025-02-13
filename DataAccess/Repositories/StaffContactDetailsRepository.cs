using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class StaffContactDetailsRepository : RepositoryBase<StaffContactDetail>, IStaffContactDetailsRepository
    {
        public StaffContactDetailsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
