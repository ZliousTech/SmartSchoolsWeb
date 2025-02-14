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
public interface IAspNetRolesManager
	{

    void Add(AspNetRole AspNetRole);

    void Update(AspNetRole AspNetRole);

    void Delete(AspNetRole AspNetRole);

    AspNetRole GetById(short id);
      
    IList<AspNetRole> GetAll();

    IEnumerable<AspNetRole> Find(Expression<Func<AspNetRole, bool>> where, params Expression<Func<AspNetRole, object>>[] includes);
		
    }

}
