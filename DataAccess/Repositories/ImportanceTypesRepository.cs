using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class ImportanceTypesRepository : RepositoryBase<ImportanceType>, IImportanceTypesRepository
    {
        public ImportanceTypesRepository(DbContext context)
            : base(context)
        {

        }
    }

}
