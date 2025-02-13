using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class AspNetRolesRepository : RepositoryBase<AspNetRole>, IAspNetRolesRepository
    {
        public AspNetRolesRepository(DbContext context)
            : base(context)
        {

        }
    }

}
