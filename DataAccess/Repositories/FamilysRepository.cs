using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class FamilysRepository : RepositoryBase<Family>, IFamilysRepository
    {
        public FamilysRepository(DbContext context)
            : base(context)
        {

        }
    }

}
