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
public interface IStudentPaymentsManager
	{

    void Add(StudentPayment StudentPayment);

    void Update(StudentPayment StudentPayment);

    void Delete(StudentPayment StudentPayment);

    StudentPayment GetById(short id);
      
    IList<StudentPayment> GetAll();

    IEnumerable<StudentPayment> Find(Expression<Func<StudentPayment, bool>> where, params Expression<Func<StudentPayment, object>>[] includes);
		
    }

}
