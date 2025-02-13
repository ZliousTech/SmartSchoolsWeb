using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class SpecializationsRepository : RepositoryBase<Specialization>, ISpecializationsRepository
    {
        public SpecializationsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
