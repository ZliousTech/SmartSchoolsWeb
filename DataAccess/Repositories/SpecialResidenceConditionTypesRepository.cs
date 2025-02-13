using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class SpecialResidenceConditionTypesRepository : RepositoryBase<SpecialResidenceConditionType>, ISpecialResidenceConditionTypesRepository
    {
        public SpecialResidenceConditionTypesRepository(DbContext context)
            : base(context)
        {

        }
    }

}
