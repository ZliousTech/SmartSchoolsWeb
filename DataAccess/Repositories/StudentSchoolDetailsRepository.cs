using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class StudentSchoolDetailsRepository : RepositoryBase<StudentSchoolDetail>, IStudentSchoolDetailsRepository
    {
        public StudentSchoolDetailsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
