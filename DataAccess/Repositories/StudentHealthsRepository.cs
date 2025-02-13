using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class StudentHealthsRepository : RepositoryBase<StudentHealth>, IStudentHealthsRepository
    {
        public StudentHealthsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
