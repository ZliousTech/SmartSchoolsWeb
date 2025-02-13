using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class ExternalGuardiansRepository : RepositoryBase<ExternalGuardian>, IExternalGuardiansRepository
    {
        public ExternalGuardiansRepository(DbContext context)
            : base(context)
        {

        }
    }

}
