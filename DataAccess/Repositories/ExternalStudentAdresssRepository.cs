using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class ExternalStudentAdresssRepository : RepositoryBase<ExternalStudentAdress>, IExternalStudentAdresssRepository
    {
        public ExternalStudentAdresssRepository(DbContext context)
            : base(context)
        {

        }
    }

}
