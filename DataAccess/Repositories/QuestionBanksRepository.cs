using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class QuestionBanksRepository : RepositoryBase<QuestionBank>, IQuestionBanksRepository
    {
        public QuestionBanksRepository(DbContext context)
            : base(context)
        {

        }
    }

}
