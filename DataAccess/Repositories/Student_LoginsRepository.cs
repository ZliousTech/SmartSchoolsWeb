using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class Student_LoginsRepository : RepositoryBase<Student_Login>, IStudent_LoginsRepository
    {
        public Student_LoginsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
