 

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
internal class StudentHealthsRepository : RepositoryBase<StudentHealth> ,IStudentHealthsRepository
	{
    public StudentHealthsRepository(DbContext context)
		: base(context)
    {
           
    }
}

}
