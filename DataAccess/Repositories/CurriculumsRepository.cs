using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class CurriculumsRepository : RepositoryBase<Curriculum>, ICurriculumsRepository
    {
        public CurriculumsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
