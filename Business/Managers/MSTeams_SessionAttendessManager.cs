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
public class MSTeams_SessionAttendessManager : BusinessComponentBase, IMSTeams_SessionAttendessManager
{


    public MSTeams_SessionAttendessManager(IDataAccessFactory dataAccessFactory)
        : base(dataAccessFactory)
    {

    }

public void Add(MSTeams_SessionAttendes MSTeams_SessionAttendes)
    {

DataAccessFactory.GetMSTeams_SessionAttendessRepository.Add(MSTeams_SessionAttendes);
            DataAccessFactory.Commit();

}

public void Update(MSTeams_SessionAttendes MSTeams_SessionAttendes)
    {

DataAccessFactory.GetMSTeams_SessionAttendessRepository.Update(MSTeams_SessionAttendes);
        DataAccessFactory.Commit();

}

public void Delete(MSTeams_SessionAttendes MSTeams_SessionAttendes)
    {

DataAccessFactory.GetMSTeams_SessionAttendessRepository.Delete(MSTeams_SessionAttendes);
            DataAccessFactory.Commit();

}

public MSTeams_SessionAttendes GetById(short id)
    {
        MSTeams_SessionAttendes MSTeams_SessionAttendes = null;

        //  MSTeams_SessionAttendes =  DataAccessFactory.GetMSTeams_SessionAttendessRepository.First(_MSTeams_SessionAttendes => _MSTeams_SessionAttendes.MSTeams_SessionAttendesID == id);

        return MSTeams_SessionAttendes;
    }

    public IList<MSTeams_SessionAttendes> GetAll()
    {
        IEnumerable<MSTeams_SessionAttendes> MSTeams_SessionAttendess = null;

        MSTeams_SessionAttendess = DataAccessFactory.GetMSTeams_SessionAttendessRepository.GetAll();

        IList<MSTeams_SessionAttendes> result = MSTeams_SessionAttendess.ToList<MSTeams_SessionAttendes>();

        return result;
    }
		
    public IEnumerable<MSTeams_SessionAttendes> Find(Expression<Func<MSTeams_SessionAttendes, bool>> @where, params Expression<Func<MSTeams_SessionAttendes, object>>[] includes)
    {
        return  DataAccessFactory.GetMSTeams_SessionAttendessRepository.Find(@where, includes);
    }
		



        		

}

}
