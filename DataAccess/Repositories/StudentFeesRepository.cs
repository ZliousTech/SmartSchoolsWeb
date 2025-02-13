using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class StudentFeesRepository : RepositoryBase<StudentFee>, IStudentFeesRepository
    {
        public StudentFeesRepository(DbContext context)
            : base(context)
        {

        }
    }

}
