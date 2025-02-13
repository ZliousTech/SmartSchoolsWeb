using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class LanguagesRepository : RepositoryBase<Language>, ILanguagesRepository
    {
        public LanguagesRepository(DbContext context)
            : base(context)
        {

        }
    }

}
