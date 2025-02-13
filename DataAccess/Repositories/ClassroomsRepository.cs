using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class ClassroomsRepository : RepositoryBase<Classroom>, IClassroomsRepository
    {
        public ClassroomsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
