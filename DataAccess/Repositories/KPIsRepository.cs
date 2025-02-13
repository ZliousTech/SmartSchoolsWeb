using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class KPIsRepository : RepositoryBase<KPI>, IKPIsRepository
    {
        public KPIsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
