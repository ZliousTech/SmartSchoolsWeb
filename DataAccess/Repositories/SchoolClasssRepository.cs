using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class SchoolClasssRepository : RepositoryBase<SchoolClass>, ISchoolClasssRepository
    {
        public SchoolClasssRepository(DbContext context)
            : base(context)
        {

        }
    }

}
