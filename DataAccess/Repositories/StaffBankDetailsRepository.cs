using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class StaffBankDetailsRepository : RepositoryBase<StaffBankDetail>, IStaffBankDetailsRepository
    {
        public StaffBankDetailsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
