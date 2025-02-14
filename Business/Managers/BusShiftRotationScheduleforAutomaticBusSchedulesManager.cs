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
public class BusShiftRotationScheduleforAutomaticBusSchedulesManager : BusinessComponentBase, IBusShiftRotationScheduleforAutomaticBusSchedulesManager
{


    public BusShiftRotationScheduleforAutomaticBusSchedulesManager(IDataAccessFactory dataAccessFactory)
        : base(dataAccessFactory)
    {

    }

public void Add(BusShiftRotationScheduleforAutomaticBusSchedule BusShiftRotationScheduleforAutomaticBusSchedule)
    {

DataAccessFactory.GetBusShiftRotationScheduleforAutomaticBusSchedulesRepository.Add(BusShiftRotationScheduleforAutomaticBusSchedule);
            DataAccessFactory.Commit();

}

public void Update(BusShiftRotationScheduleforAutomaticBusSchedule BusShiftRotationScheduleforAutomaticBusSchedule)
    {

DataAccessFactory.GetBusShiftRotationScheduleforAutomaticBusSchedulesRepository.Update(BusShiftRotationScheduleforAutomaticBusSchedule);
        DataAccessFactory.Commit();

}

public void Delete(BusShiftRotationScheduleforAutomaticBusSchedule BusShiftRotationScheduleforAutomaticBusSchedule)
    {

DataAccessFactory.GetBusShiftRotationScheduleforAutomaticBusSchedulesRepository.Delete(BusShiftRotationScheduleforAutomaticBusSchedule);
            DataAccessFactory.Commit();

}

public BusShiftRotationScheduleforAutomaticBusSchedule GetById(short id)
    {
        BusShiftRotationScheduleforAutomaticBusSchedule BusShiftRotationScheduleforAutomaticBusSchedule = null;

        //  BusShiftRotationScheduleforAutomaticBusSchedule =  DataAccessFactory.GetBusShiftRotationScheduleforAutomaticBusSchedulesRepository.First(_BusShiftRotationScheduleforAutomaticBusSchedule => _BusShiftRotationScheduleforAutomaticBusSchedule.BusShiftRotationScheduleforAutomaticBusScheduleID == id);

        return BusShiftRotationScheduleforAutomaticBusSchedule;
    }

    public IList<BusShiftRotationScheduleforAutomaticBusSchedule> GetAll()
    {
        IEnumerable<BusShiftRotationScheduleforAutomaticBusSchedule> BusShiftRotationScheduleforAutomaticBusSchedules = null;

        BusShiftRotationScheduleforAutomaticBusSchedules = DataAccessFactory.GetBusShiftRotationScheduleforAutomaticBusSchedulesRepository.GetAll();

        IList<BusShiftRotationScheduleforAutomaticBusSchedule> result = BusShiftRotationScheduleforAutomaticBusSchedules.ToList<BusShiftRotationScheduleforAutomaticBusSchedule>();

        return result;
    }
		
    public IEnumerable<BusShiftRotationScheduleforAutomaticBusSchedule> Find(Expression<Func<BusShiftRotationScheduleforAutomaticBusSchedule, bool>> @where, params Expression<Func<BusShiftRotationScheduleforAutomaticBusSchedule, object>>[] includes)
    {
        return  DataAccessFactory.GetBusShiftRotationScheduleforAutomaticBusSchedulesRepository.Find(@where, includes);
    }
		



        		

}

}
