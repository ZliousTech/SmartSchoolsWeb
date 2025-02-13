using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using System.Data.Entity;

namespace DataAccess.Repositories
{
    internal class TeacherExperiencesRepository : RepositoryBase<TeacherExperience>, ITeacherExperiencesRepository
    {
        public TeacherExperiencesRepository(DbContext context)
            : base(context)
        {

        }
    }

}
