using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class EducationalYearsRepository : RepositoryBase<EducationalYear>, IEducationalYearsRepository
    {
        public EducationalYearsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
