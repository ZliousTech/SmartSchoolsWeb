using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class DesignationTypesRepository : RepositoryBase<DesignationType>, IDesignationTypesRepository
    {
        public DesignationTypesRepository(DbContext context)
            : base(context)
        {

        }
    }

}
