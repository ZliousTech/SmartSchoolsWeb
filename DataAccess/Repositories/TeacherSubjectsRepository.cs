using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class TeacherSubjectsRepository : RepositoryBase<TeacherSubject>, ITeacherSubjectsRepository
    {
        public TeacherSubjectsRepository(DbContext context)
            : base(context)
        {

        }
    }

}
