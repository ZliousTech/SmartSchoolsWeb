using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class GuardiansRepository : RepositoryBase<Guardian>, IGuardiansRepository
    {
        public GuardiansRepository(DbContext context)
            : base(context)
        {

        }
    }

}
