using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class CurriculumDepartmentsRepository : RepositoryBase<CurriculumDepartment>, ICurriculumDepartmentsRepository
    {
        public CurriculumDepartmentsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
