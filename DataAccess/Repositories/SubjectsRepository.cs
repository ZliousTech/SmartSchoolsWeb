using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class SubjectsRepository : RepositoryBase<Subject>, ISubjectsRepository
    {
        public SubjectsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
