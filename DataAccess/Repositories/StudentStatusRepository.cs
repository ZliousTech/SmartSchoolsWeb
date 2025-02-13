using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class StudentStatusRepository : RepositoryBase<StudentStatu>, IStudentStatusRepository
    {
        public StudentStatusRepository(DbContext context)
            : base(context)
        {

        }
    }

}
