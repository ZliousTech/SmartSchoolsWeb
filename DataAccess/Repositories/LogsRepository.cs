using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class LogsRepository : RepositoryBase<Log>, ILogsRepository
    {
        public LogsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
