using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class GroupsRepository : RepositoryBase<Group>, IGroupsRepository
    {
        public GroupsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
