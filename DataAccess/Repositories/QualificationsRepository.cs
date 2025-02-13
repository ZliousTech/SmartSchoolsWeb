using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class QualificationsRepository : RepositoryBase<Qualification>, IQualificationsRepository
    {
        public QualificationsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
