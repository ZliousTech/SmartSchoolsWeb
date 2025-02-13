using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class MSTeams_SessionssRepository : RepositoryBase<MSTeams_Sessions>, IMSTeams_SessionssRepository
    {
        public MSTeams_SessionssRepository(DbContext context)
            : base(context)
        {

        }
    }

}
