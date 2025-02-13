using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class StudentResultsRepository : RepositoryBase<StudentResult>, IStudentResultsRepository
    {
        public StudentResultsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
