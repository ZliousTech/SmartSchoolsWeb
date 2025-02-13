using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class DocumentsRepository : RepositoryBase<Document>, IDocumentsRepository
    {
        public DocumentsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
