using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class NotificationsWebsRepository : RepositoryBase<NotificationsWeb>, INotificationsWebsRepository
    {
        public NotificationsWebsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
