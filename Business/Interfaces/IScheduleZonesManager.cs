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
public interface IScheduleZonesManager
	{

    void Add(ScheduleZone ScheduleZone);

    void Update(ScheduleZone ScheduleZone);

    void Delete(ScheduleZone ScheduleZone);

    ScheduleZone GetById(short id);
      
    IList<ScheduleZone> GetAll();

    IEnumerable<ScheduleZone> Find(Expression<Func<ScheduleZone, bool>> where, params Expression<Func<ScheduleZone, object>>[] includes);
		
    }

}
