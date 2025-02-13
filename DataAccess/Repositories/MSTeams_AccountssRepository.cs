using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class MSTeams_AccountssRepository : RepositoryBase<MSTeams_Accounts>, IMSTeams_AccountssRepository
    {
        public MSTeams_AccountssRepository(DbContext context)
            : base(context)
        {

        }
    }

}
