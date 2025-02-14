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
public class FunctionsManager : BusinessComponentBase, IFunctionsManager
{


    public FunctionsManager(IDataAccessFactory dataAccessFactory)
        : base(dataAccessFactory)
    {

    }

public void Add(Function Function)
    {

DataAccessFactory.GetFunctionsRepository.Add(Function);
            DataAccessFactory.Commit();

}

public void Update(Function Function)
    {

DataAccessFactory.GetFunctionsRepository.Update(Function);
        DataAccessFactory.Commit();

}

public void Delete(Function Function)
    {

DataAccessFactory.GetFunctionsRepository.Delete(Function);
            DataAccessFactory.Commit();

}

public Function GetById(short id)
    {
        Function Function = null;

        //  Function =  DataAccessFactory.GetFunctionsRepository.First(_Function => _Function.FunctionID == id);

        return Function;
    }

    public IList<Function> GetAll()
    {
        IEnumerable<Function> Functions = null;

        Functions = DataAccessFactory.GetFunctionsRepository.GetAll();

        IList<Function> result = Functions.ToList<Function>();

        return result;
    }
		
    public IEnumerable<Function> Find(Expression<Func<Function, bool>> @where, params Expression<Func<Function, object>>[] includes)
    {
        return  DataAccessFactory.GetFunctionsRepository.Find(@where, includes);
    }
		



        		

}

}
