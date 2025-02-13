using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class CustomPropertyTypesRepository : RepositoryBase<CustomPropertyType>, ICustomPropertyTypesRepository
    {
        public CustomPropertyTypesRepository(DbContext context)
            : base(context)
        {

        }
    }

}
