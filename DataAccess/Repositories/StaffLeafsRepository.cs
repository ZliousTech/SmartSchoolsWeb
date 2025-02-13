using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class StaffLeafsRepository : RepositoryBase<StaffLeaf>, IStaffLeafsRepository
    {
        public StaffLeafsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
