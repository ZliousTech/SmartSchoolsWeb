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
public interface IAccountsDetailsManager
	{

    void Add(AccountsDetail AccountsDetail);

    void Update(AccountsDetail AccountsDetail);

    void Delete(AccountsDetail AccountsDetail);

    AccountsDetail GetById(short id);
      
    IList<AccountsDetail> GetAll();

    IEnumerable<AccountsDetail> Find(Expression<Func<AccountsDetail, bool>> where, params Expression<Func<AccountsDetail, object>>[] includes);
		
    }

}
