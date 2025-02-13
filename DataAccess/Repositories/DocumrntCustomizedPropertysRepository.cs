using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class DocumrntCustomizedPropertysRepository : RepositoryBase<DocumrntCustomizedProperty>, IDocumrntCustomizedPropertysRepository
    {
        public DocumrntCustomizedPropertysRepository(DbContext context)
            : base(context)
        {

        }
    }

}
