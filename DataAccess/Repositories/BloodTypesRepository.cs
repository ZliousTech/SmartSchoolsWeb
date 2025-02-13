using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class BloodTypesRepository : RepositoryBase<BloodType>, IBloodTypesRepository
    {
        public BloodTypesRepository(DbContext context)
            : base(context)
        {

        }
    }

}
