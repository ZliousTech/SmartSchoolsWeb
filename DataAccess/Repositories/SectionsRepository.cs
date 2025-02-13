using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class SectionsRepository : RepositoryBase<Section>, ISectionsRepository
    {
        public SectionsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
