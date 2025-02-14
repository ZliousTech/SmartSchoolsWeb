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
using Objects.DTO;

namespace Business.Interfaces
{
    public interface IStaffsManager
    {

        void Add(Staff Staff);

        void Update(Staff Staff);

        void Delete(Staff Staff);

        Staff GetById(short id);

        IList<Staff> GetAll();

        IEnumerable<Staff> Find(Expression<Func<Staff, bool>> where, params Expression<Func<Staff, object>>[] includes);
        List<StaffDesignationDTO> GetEmployeeStatisticsByDesgination(int SchoolID);
        List<EmployeeDTO> GetEmployees(int SchoolID, string lang);
        List<TeacherDTO> GetTeachers(int SchoolID, string lang, string schoolyear);
        List<EmployeeDTO> GetDrivers(int SchoolID, string lang);
        List<EmployeeDTO> GetEscort(int SchoolID, string lang);

    }

}
