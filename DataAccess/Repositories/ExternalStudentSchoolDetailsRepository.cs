using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class ExternalStudentSchoolDetailsRepository : RepositoryBase<ExternalStudentSchoolDetail>, IExternalStudentSchoolDetailsRepository
    {
        public ExternalStudentSchoolDetailsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
