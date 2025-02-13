using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class UserTypesRepository : RepositoryBase<UserType>, IUserTypesRepository
    {
        public UserTypesRepository(DbContext context)
            : base(context)
        {

        }
    }

}
