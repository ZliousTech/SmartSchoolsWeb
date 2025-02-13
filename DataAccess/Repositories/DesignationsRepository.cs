using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class DesignationsRepository : RepositoryBase<Designation>, IDesignationsRepository
    {
        public DesignationsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
