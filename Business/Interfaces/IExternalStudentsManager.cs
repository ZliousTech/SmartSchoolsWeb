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
public interface IExternalStudentsManager
	{

    void Add(ExternalStudent ExternalStudent);

    void Update(ExternalStudent ExternalStudent);

    void Delete(ExternalStudent ExternalStudent);

    ExternalStudent GetById(short id);
      
    IList<ExternalStudent> GetAll();

    IEnumerable<ExternalStudent> Find(Expression<Func<ExternalStudent, bool>> where, params Expression<Func<ExternalStudent, object>>[] includes);
		
    }

}
