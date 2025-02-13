using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class StudentAdresssRepository : RepositoryBase<StudentAdress>, IStudentAdresssRepository
    {
        public StudentAdresssRepository(DbContext context)
            : base(context)
        {

        }
    }

}
