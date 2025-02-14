//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
// </auto-generated>
//------------------------------------------------------------------------------
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Data.SqlClient;


using DataAccess;
using DataAccess.Base;
using Business.Base;
using Business.Interfaces;
using Objects;


 
namespace Business.Managers
{
public class AccountsMastersManager : BusinessComponentBase, IAccountsMastersManager
{


    public AccountsMastersManager(IDataAccessFactory dataAccessFactory)
        : base(dataAccessFactory)
    {

    }

public void Add(AccountsMaster AccountsMaster)
    {

DataAccessFactory.GetAccountsMastersRepository.Add(AccountsMaster);
            DataAccessFactory.Commit();

}

public void Update(AccountsMaster AccountsMaster)
    {

DataAccessFactory.GetAccountsMastersRepository.Update(AccountsMaster);
        DataAccessFactory.Commit();

}

public void Delete(AccountsMaster AccountsMaster)
    {

DataAccessFactory.GetAccountsMastersRepository.Delete(AccountsMaster);
            DataAccessFactory.Commit();

}

public AccountsMaster GetById(short id)
    {
        AccountsMaster AccountsMaster = null;

        //  AccountsMaster =  DataAccessFactory.GetAccountsMastersRepository.First(_AccountsMaster => _AccountsMaster.AccountsMasterID == id);

        return AccountsMaster;
    }

    public IList<AccountsMaster> GetAll()
    {
        IEnumerable<AccountsMaster> AccountsMasters = null;

        AccountsMasters = DataAccessFactory.GetAccountsMastersRepository.GetAll();

        IList<AccountsMaster> result = AccountsMasters.ToList<AccountsMaster>();

        return result;
    }
		
    public IEnumerable<AccountsMaster> Find(Expression<Func<AccountsMaster, bool>> @where, params Expression<Func<AccountsMaster, object>>[] includes)
    {
        return  DataAccessFactory.GetAccountsMastersRepository.Find(@where, includes);
    }
		



        		

}

}
