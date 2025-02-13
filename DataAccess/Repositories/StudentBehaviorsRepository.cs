using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class StudentBehaviorsRepository : RepositoryBase<StudentBehavior>, IStudentBehaviorsRepository
    {
        public StudentBehaviorsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
