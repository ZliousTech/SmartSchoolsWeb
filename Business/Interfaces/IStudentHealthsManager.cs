//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess;
using System.Linq.Expressions;
using Objects;
namespace Business.Interfaces
{
public interface IStudentHealthsManager
	{

    void Add(StudentHealth StudentHealth);

    void Update(StudentHealth StudentHealth);

    void Delete(StudentHealth StudentHealth);

    StudentHealth GetById(short id);
      
    IList<StudentHealth> GetAll();

    IEnumerable<StudentHealth> Find(Expression<Func<StudentHealth, bool>> where, params Expression<Func<StudentHealth, object>>[] includes);
		
    }

}






