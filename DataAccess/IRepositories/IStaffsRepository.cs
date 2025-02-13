using DataAccess.Base;
using Objects;
using Objects.DTO;
using System.Collections.Generic;

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
