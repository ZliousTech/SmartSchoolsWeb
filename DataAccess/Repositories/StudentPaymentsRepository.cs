using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class StudentPaymentsRepository : RepositoryBase<StudentPayment>, IStudentPaymentsRepository
    {
        public StudentPaymentsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
