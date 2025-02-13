using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class StudentDiseassRepository : RepositoryBase<StudentDiseas>, IStudentDiseassRepository
    {
        public StudentDiseassRepository(DbContext context)
            : base(context)
        {

        }
    }

}
