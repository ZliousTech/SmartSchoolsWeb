using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class NotificationsRepository : RepositoryBase<Notification>, INotificationsRepository
    {
        public NotificationsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
