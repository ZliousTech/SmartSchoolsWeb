using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataAccess.Base;
using DataAccess;
using Objects;
using Objects.DTO;

namespace DataAccess.IRepositories
{
public interface IStaffsRepository : IRepository<Staff>
{
        List<StaffDesignationDTO> GetEmployeeStatisticsByDesgination(int SchoolID);
        List<EmployeeDTO> GetEmployees(int SchoolID, string lang);
        List<TeacherDTO> GetTeachers(int SchoolID, string lang, string schoolyear);
        List<EmployeeDTO> GetDrivers(int SchoolID, string lang);
        List<EmployeeDTO> GetEscort(int SchoolID, string lang);
    }

}
