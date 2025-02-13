using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class MailTemplatesRepository : RepositoryBase<MailTemplate>, IMailTemplatesRepository
    {
        public MailTemplatesRepository(DbContext context)
            : base(context)
        {

        }
    }

}
