using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class DocumentCustomPropertysRepository : RepositoryBase<DocumentCustomProperty>, IDocumentCustomPropertysRepository
    {
        public DocumentCustomPropertysRepository(DbContext context)
            : base(context)
        {

        }
    }

}
