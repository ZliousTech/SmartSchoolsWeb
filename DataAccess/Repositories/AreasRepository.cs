using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class AreasRepository : RepositoryBase<Area>, IAreasRepository
    {
        public AreasRepository(DbContext context)
            : base(context)
        {

        }
    }

}
