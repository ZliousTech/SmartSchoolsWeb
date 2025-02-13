using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class DocumentTypesRepository : RepositoryBase<DocumentType>, IDocumentTypesRepository
    {
        public DocumentTypesRepository(DbContext context)
            : base(context)
        {

        }
    }

}
