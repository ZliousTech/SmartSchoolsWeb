using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class TeachersRepository : RepositoryBase<Teacher>, ITeachersRepository
    {
        public TeachersRepository(DbContext context)
            : base(context)
        {

        }
    }

}
