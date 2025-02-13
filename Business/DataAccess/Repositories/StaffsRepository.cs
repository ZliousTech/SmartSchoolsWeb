

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using DataAccess.Base;
using DataAccess.IRepositories;
using Objects;
using Objects.DTO;

namespace DataAccess.Repositories
{
    internal class StaffsRepository : RepositoryBase<Staff>, IStaffsRepository
    {
        SmartSchoolsEntities context = new SmartSchoolsEntities();

        public StaffsRepository(DbContext context)
        : base(context)
        {

        }
        public List<StaffDesignationDTO> GetEmployeeStatisticsByDesgination(int SchoolID)
        {
            var result = from Staff in context.Staffs
                         join StaffJobDetail in context.StaffJobDetails
                         on Staff.StaffID equals StaffJobDetail.StaffID
                         join Designation in context.Designations on StaffJobDetail.Designation
                         equals Designation.DesignationID

                         where Staff.Active == -1 && StaffJobDetail.SchoolID == SchoolID
                         select new StaffDesignationDTO
                         {
                             SchooolID = StaffJobDetail.SchoolID.Value,
                             StaffID = Staff.StaffID,
                             DesignationID = Designation.DesignationID,
                             DesignationArabicText = Designation.DesignationArabicText,
                             DesignationEnglishText = Designation.DesignationEnglishText
                         };
            return result.ToList();
        }

        public List<EmployeeDTO> GetEmployees(int SchoolID, string lang)
        {
            var result = from Staff in context.Staffs
                         join StaffJobDetail in context.StaffJobDetails
                         on Staff.StaffID equals StaffJobDetail.StaffID
                         join StaffContactDetail in context.StaffContactDetails
                         on Staff.StaffID equals StaffContactDetail.StaffID
                         join StaffSalaryDetail in context.StaffSalaryDetails
                         on Staff.StaffID equals StaffSalaryDetail.StaffID
                         join StaffBankDetail in context.StaffBankDetails
                         on Staff.StaffID equals StaffBankDetail.StaffID
                         join Department in context.Departments
                         on StaffJobDetail.Department equals Department.DepartmentID
                         join Designation in context.Designations
                         on StaffJobDetail.Designation equals Designation.DesignationID
                         where Staff.Active == -1 && StaffJobDetail.SchoolID == SchoolID
                         select new EmployeeDTO
                         {
                             StaffID = Staff.StaffID,
                             EmployeeName = lang == "en" ? Staff.StaffEnglishName.Replace("-", " ") : Staff.StaffArabicName.Replace("-", " "),
                             DepartmentName = lang == "en" ? Department.DepartmentEnglishName : Department.DepartmentArabicName,
                             Destigniation = lang == "en" ? Designation.DesignationEnglishText : Designation.DesignationArabicText,
                             dateofjoining = StaffJobDetail.DateOfJoining.Value,
                             Email = StaffContactDetail.Email,
                             mobilenumber = StaffContactDetail.MobileNo,
                             image = Staff.Photo,
                         };
            return result.OrderBy(a => a.EmployeeName).ToList();
        }

        public List<TeacherDTO> GetTeachers(int SchoolID, string lang, string schoolyear)
        {
            //var result = from Teacher in context.Teachers
            //             join Staff in context.Staffs
            //             on Teacher.StaffID equals Staff.StaffID
            //             join TeacherSubject in context.TeacherSubjects
            //             on Teacher.StaffID equals TeacherSubject.TeacherID
            //             where TeacherSubject.SchoolID == SchoolID
            //             //&& TeacherSubject.SchoolYear == schoolyear
            //             //&& Staff.Active == -1
            //             select new TeacherDTO
            //             {
            //                 StaffID = Staff.StaffID,
            //                 TeacherName = lang == "en" ? Staff.StaffEnglishName.Replace("-", " ") : Staff.StaffArabicName.Replace("-", " "),
            //                 NumberofSessins = Teacher.NumberofSessins,
            //                 TimeTableSessions = Teacher.TimeTableSessions

            //             };
            //return result.OrderBy(a => a.TeacherName).Distinct().ToList();

            var result = from Staff in context.Staffs
                         join StaffJob in context.StaffJobDetails
                         on Staff.StaffID equals StaffJob.StaffID
                         join Teacher in context.Teachers
                         on Staff.StaffID equals Teacher.StaffID
                         // 6 means its teacher, 12 means its first teacher.
                         where (StaffJob.Designation == 6 || StaffJob.Designation == 12) && StaffJob.SchoolID == SchoolID
                         select new TeacherDTO
                         {
                             StaffID = Staff.StaffID,
                             TeacherName = lang == "en" ? Staff.StaffEnglishName.Replace("-", " ") : Staff.StaffArabicName.Replace("-", " "),
                             NumberofSessins = Teacher.NumberofSessins,
                             TimeTableSessions = Teacher.TimeTableSessions
                         };

            return result.OrderBy(a => a.TeacherName).ToList();
        }

        public List<EmployeeDTO> GetDrivers(int SchoolID, string lang)
        {
            var result = from Staff in context.Staffs
                         join StaffJobDetail in context.StaffJobDetails
                         on Staff.StaffID equals StaffJobDetail.StaffID
                         join StaffContactDetail in context.StaffContactDetails
                         on Staff.StaffID equals StaffContactDetail.StaffID
                         join StaffSalaryDetail in context.StaffSalaryDetails
                         on Staff.StaffID equals StaffSalaryDetail.StaffID
                         join StaffBankDetail in context.StaffBankDetails
                         on Staff.StaffID equals StaffBankDetail.StaffID
                         join Department in context.Departments
                         on StaffJobDetail.Department equals Department.DepartmentID
                         where Staff.Active == -1 && StaffJobDetail.SchoolID == SchoolID && StaffJobDetail.Designation == 7
                         select new EmployeeDTO
                         {
                             StaffID = Staff.StaffID,
                             EmployeeName = lang == "en" ? Staff.StaffEnglishName.Replace("-", " ") : Staff.StaffArabicName.Replace("-", " "),
                             DepartmentName = lang == "en" ? Department.DepartmentEnglishName : Department.DepartmentArabicName,
                             dateofjoining = StaffJobDetail.DateOfJoining.Value,
                             Email = StaffContactDetail.Email,
                             mobilenumber = StaffContactDetail.MobileNo,
                             image = Staff.Photo,
                         };
            return result.OrderBy(a => a.EmployeeName).ToList();
        }

        public List<EmployeeDTO> GetEscort(int SchoolID, string lang)
        {
            var result = from Staff in context.Staffs
                         join StaffJobDetail in context.StaffJobDetails
                         on Staff.StaffID equals StaffJobDetail.StaffID
                         where Staff.Active == -1 && StaffJobDetail.SchoolID == SchoolID && StaffJobDetail.Designation == 14
                         select new EmployeeDTO
                         {
                             StaffID = Staff.StaffID,
                             EmployeeName = lang == "en" ? Staff.StaffEnglishName.Replace("-", " ") : Staff.StaffArabicName.Replace("-", " "),
                             dateofjoining = StaffJobDetail.DateOfJoining.Value,
                           
                         };
            return result.OrderBy(a => a.EmployeeName).ToList();
        }
    }

}
