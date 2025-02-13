 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
 
namespace DataAccess.Repositories
{
internal class TeacherSubjectsRepository : RepositoryBase<TeacherSubject> ,ITeacherSubjectsRepository
	{
    public TeacherSubjectsRepository(DbContext context)
		: base(context)
    {
           
    }
}

}
