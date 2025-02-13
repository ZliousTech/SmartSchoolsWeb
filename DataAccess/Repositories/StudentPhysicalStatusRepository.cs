using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class StudentPhysicalStatusRepository : RepositoryBase<StudentPhysicalStatu>, IStudentPhysicalStatusRepository
    {
        public StudentPhysicalStatusRepository(DbContext context)
            : base(context)
        {

        }
    }

}
