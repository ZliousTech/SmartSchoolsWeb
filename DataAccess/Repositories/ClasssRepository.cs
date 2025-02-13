using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class ClasssRepository : RepositoryBase<Class>, IClasssRepository
    {
        public ClasssRepository(DbContext context)
            : base(context)
        {

        }
    }

}
