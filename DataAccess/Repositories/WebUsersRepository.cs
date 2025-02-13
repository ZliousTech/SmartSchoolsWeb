using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class WebUsersRepository : RepositoryBase<WebUser>, IWebUsersRepository
    {
        public WebUsersRepository(DbContext context)
            : base(context)
        {

        }
    }

}
