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
public interface IBusToursManager
	{

    void Add(BusTour BusTour);

    void Update(BusTour BusTour);

    void Delete(BusTour BusTour);

    BusTour GetById(short id);
      
    IList<BusTour> GetAll();

    IEnumerable<BusTour> Find(Expression<Func<BusTour, bool>> where, params Expression<Func<BusTour, object>>[] includes);
		
    }

}
