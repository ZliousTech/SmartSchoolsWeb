using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class AgeClassificationsRepository : RepositoryBase<AgeClassification>, IAgeClassificationsRepository
    {
        public AgeClassificationsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
