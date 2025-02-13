using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class StaffSalaryDetailsRepository : RepositoryBase<StaffSalaryDetail>, IStaffSalaryDetailsRepository
    {
        public StaffSalaryDetailsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
