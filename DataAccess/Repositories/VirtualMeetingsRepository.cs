using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class VirtualMeetingsRepository : RepositoryBase<VirtualMeeting>, IVirtualMeetingsRepository
    {
        public VirtualMeetingsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
