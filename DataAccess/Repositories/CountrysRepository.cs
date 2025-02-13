using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class CountrysRepository : RepositoryBase<Country>, ICountrysRepository
    {
        public CountrysRepository(DbContext context)
            : base(context)
        {

        }
    }

}
