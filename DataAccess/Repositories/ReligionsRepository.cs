using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class ReligionsRepository : RepositoryBase<Religion>, IReligionsRepository
    {
        public ReligionsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
