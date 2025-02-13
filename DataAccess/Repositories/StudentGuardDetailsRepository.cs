using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class StudentGuardDetailsRepository : RepositoryBase<StudentGuardDetail>, IStudentGuardDetailsRepository
    {
        public StudentGuardDetailsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
