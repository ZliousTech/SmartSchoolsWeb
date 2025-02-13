using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class DepartmentsRepository : RepositoryBase<Department>, IDepartmentsRepository
    {
        public DepartmentsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
