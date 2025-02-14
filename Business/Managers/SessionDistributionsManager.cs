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
public class SessionDistributionsManager : BusinessComponentBase, ISessionDistributionsManager
{


    public SessionDistributionsManager(IDataAccessFactory dataAccessFactory)
        : base(dataAccessFactory)
    {

    }

public void Add(SessionDistribution SessionDistribution)
    {

DataAccessFactory.GetSessionDistributionsRepository.Add(SessionDistribution);
            DataAccessFactory.Commit();

}

public void Update(SessionDistribution SessionDistribution)
    {

DataAccessFactory.GetSessionDistributionsRepository.Update(SessionDistribution);
        DataAccessFactory.Commit();

}

public void Delete(SessionDistribution SessionDistribution)
    {

DataAccessFactory.GetSessionDistributionsRepository.Delete(SessionDistribution);
            DataAccessFactory.Commit();

}

public SessionDistribution GetById(short id)
    {
        SessionDistribution SessionDistribution = null;

        //  SessionDistribution =  DataAccessFactory.GetSessionDistributionsRepository.First(_SessionDistribution => _SessionDistribution.SessionDistributionID == id);

        return SessionDistribution;
    }

    public IList<SessionDistribution> GetAll()
    {
        IEnumerable<SessionDistribution> SessionDistributions = null;

        SessionDistributions = DataAccessFactory.GetSessionDistributionsRepository.GetAll();

        IList<SessionDistribution> result = SessionDistributions.ToList<SessionDistribution>();

        return result;
    }
		
    public IEnumerable<SessionDistribution> Find(Expression<Func<SessionDistribution, bool>> @where, params Expression<Func<SessionDistribution, object>>[] includes)
    {
        return  DataAccessFactory.GetSessionDistributionsRepository.Find(@where, includes);
    }
		



        		

}

}
