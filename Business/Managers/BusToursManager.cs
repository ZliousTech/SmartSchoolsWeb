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
public class BusToursManager : BusinessComponentBase, IBusToursManager
{


    public BusToursManager(IDataAccessFactory dataAccessFactory)
        : base(dataAccessFactory)
    {

    }

public void Add(BusTour BusTour)
    {

DataAccessFactory.GetBusToursRepository.Add(BusTour);
            DataAccessFactory.Commit();

}

public void Update(BusTour BusTour)
    {

DataAccessFactory.GetBusToursRepository.Update(BusTour);
        DataAccessFactory.Commit();

}

public void Delete(BusTour BusTour)
    {

DataAccessFactory.GetBusToursRepository.Delete(BusTour);
            DataAccessFactory.Commit();

}

public BusTour GetById(short id)
    {
        BusTour BusTour = null;

        //  BusTour =  DataAccessFactory.GetBusToursRepository.First(_BusTour => _BusTour.BusTourID == id);

        return BusTour;
    }

    public IList<BusTour> GetAll()
    {
        IEnumerable<BusTour> BusTours = null;

        BusTours = DataAccessFactory.GetBusToursRepository.GetAll();

        IList<BusTour> result = BusTours.ToList<BusTour>();

        return result;
    }
		
    public IEnumerable<BusTour> Find(Expression<Func<BusTour, bool>> @where, params Expression<Func<BusTour, object>>[] includes)
    {
        return  DataAccessFactory.GetBusToursRepository.Find(@where, includes);
    }
		



        		

}

}
