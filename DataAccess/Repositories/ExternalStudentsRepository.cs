using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class ExternalStudentsRepository : RepositoryBase<ExternalStudent>, IExternalStudentsRepository
    {
        public ExternalStudentsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
