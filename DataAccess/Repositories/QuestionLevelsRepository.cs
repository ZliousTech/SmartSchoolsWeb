using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class QuestionLevelsRepository : RepositoryBase<QuestionLevel>, IQuestionLevelsRepository
    {
        public QuestionLevelsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
