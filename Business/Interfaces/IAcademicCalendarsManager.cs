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
public interface IAcademicCalendarsManager
	{

    void Add(AcademicCalendar AcademicCalendar);

    void Update(AcademicCalendar AcademicCalendar);

    void Delete(AcademicCalendar AcademicCalendar);

    AcademicCalendar GetById(short id);
      
    IList<AcademicCalendar> GetAll();

    IEnumerable<AcademicCalendar> Find(Expression<Func<AcademicCalendar, bool>> where, params Expression<Func<AcademicCalendar, object>>[] includes);
		
    }

}
