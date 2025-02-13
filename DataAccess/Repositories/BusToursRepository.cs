using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class BusToursRepository : RepositoryBase<BusTour>, IBusToursRepository
    {
        public BusToursRepository(DbContext context)
            : base(context)
        {

        }
    }

}
