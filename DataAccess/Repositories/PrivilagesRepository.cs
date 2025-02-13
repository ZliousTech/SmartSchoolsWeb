using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class PrivilagesRepository : RepositoryBase<Privilage>, IPrivilagesRepository
    {
        public PrivilagesRepository(DbContext context)
            : base(context)
        {

        }
    }

}
