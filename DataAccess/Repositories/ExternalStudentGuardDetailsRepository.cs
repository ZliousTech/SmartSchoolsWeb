using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class ExternalStudentGuardDetailsRepository : RepositoryBase<ExternalStudentGuardDetail>, IExternalStudentGuardDetailsRepository
    {
        public ExternalStudentGuardDetailsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
