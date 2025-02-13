using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class RecurrenceTypesRepository : RepositoryBase<RecurrenceType>, IRecurrenceTypesRepository
    {
        public RecurrenceTypesRepository(DbContext context)
            : base(context)
        {

        }
    }

}
