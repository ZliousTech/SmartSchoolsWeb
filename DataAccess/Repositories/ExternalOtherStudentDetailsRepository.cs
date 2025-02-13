using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class ExternalOtherStudentDetailsRepository : RepositoryBase<ExternalOtherStudentDetail>, IExternalOtherStudentDetailsRepository
    {
        public ExternalOtherStudentDetailsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
