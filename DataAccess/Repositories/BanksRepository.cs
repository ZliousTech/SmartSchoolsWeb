using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class BanksRepository : RepositoryBase<Bank>, IBanksRepository
    {
        public BanksRepository(DbContext context)
            : base(context)
        {

        }
    }

}
