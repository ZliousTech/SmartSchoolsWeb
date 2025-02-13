using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class OtherStudentDetailsRepository : RepositoryBase<OtherStudentDetail>, IOtherStudentDetailsRepository
    {
        public OtherStudentDetailsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
