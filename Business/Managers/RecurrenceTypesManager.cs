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
public class RecurrenceTypesManager : BusinessComponentBase, IRecurrenceTypesManager
{


    public RecurrenceTypesManager(IDataAccessFactory dataAccessFactory)
        : base(dataAccessFactory)
    {

    }

public void Add(RecurrenceType RecurrenceType)
    {

DataAccessFactory.GetRecurrenceTypesRepository.Add(RecurrenceType);
            DataAccessFactory.Commit();

}

public void Update(RecurrenceType RecurrenceType)
    {

DataAccessFactory.GetRecurrenceTypesRepository.Update(RecurrenceType);
        DataAccessFactory.Commit();

}

public void Delete(RecurrenceType RecurrenceType)
    {

DataAccessFactory.GetRecurrenceTypesRepository.Delete(RecurrenceType);
            DataAccessFactory.Commit();

}

public RecurrenceType GetById(short id)
    {
        RecurrenceType RecurrenceType = null;

        //  RecurrenceType =  DataAccessFactory.GetRecurrenceTypesRepository.First(_RecurrenceType => _RecurrenceType.RecurrenceTypeID == id);

        return RecurrenceType;
    }

    public IList<RecurrenceType> GetAll()
    {
        IEnumerable<RecurrenceType> RecurrenceTypes = null;

        RecurrenceTypes = DataAccessFactory.GetRecurrenceTypesRepository.GetAll();

        IList<RecurrenceType> result = RecurrenceTypes.ToList<RecurrenceType>();

        return result;
    }
		
    public IEnumerable<RecurrenceType> Find(Expression<Func<RecurrenceType, bool>> @where, params Expression<Func<RecurrenceType, object>>[] includes)
    {
        return  DataAccessFactory.GetRecurrenceTypesRepository.Find(@where, includes);
    }
		



        		

}

}
