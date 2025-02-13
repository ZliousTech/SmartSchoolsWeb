using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class TemplateSettingsRepository : RepositoryBase<TemplateSetting>, ITemplateSettingsRepository
    {
        public TemplateSettingsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
