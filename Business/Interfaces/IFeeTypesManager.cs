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
public interface IFeeTypesManager
	{

    void Add(FeeType FeeType);

    void Update(FeeType FeeType);

    void Delete(FeeType FeeType);

    FeeType GetById(short id);
      
    IList<FeeType> GetAll();

    IEnumerable<FeeType> Find(Expression<Func<FeeType, bool>> where, params Expression<Func<FeeType, object>>[] includes);
		
    }

}
