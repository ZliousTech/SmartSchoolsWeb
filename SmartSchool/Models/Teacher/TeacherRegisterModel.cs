using Common.Helpers;
using Objects.DTO;
using SmartSchool.Models.System;
using System;
using System.Collections.Generic;

namespace SmartSchool.Models.Teacher
{
    public class TeacherRegisterModel : SystemSettingsModel
    {
        public List<LookupDTO> schoolList { get; set; }
        public List<LookupDTO> TeacherNameList { get; set; }
        public List<LookupDTO> ClassroomList { get; set; }
        public List<LookupDTO> EmployeeList { get; set; }
        public List<LookupDTO> SchoolClassList { get; set; }
        public List<LookupDTO> SchoolClassList2 { get; set; }
        public List<LookupDTO> DepartmentsList { get; set; }
        public List<LookupDTO> SpecializationList { get; set; }
        public List<LookupDTO> QualificationList { get; set; }
        public List<LookupDTO> WeekDaysList { get; set; }
        public List<LookupDTO> SessionsPerDayWithStartTimeList { get; set; }
        public List<LookupDTO> SessionNameList { get; set; }
        public List<TeacherTimeTable> TeacherTimeTableList { get; set; }
        public string NationalNumber { get; set; }
        public string TeacherName { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public int? NumberofSessins { get; set; }
        public int schoolID { get; set; }
        public string StaffID { get; set; }
        public string TeacherID { get; set; }
        public int AssistanceStaffID { get; set; }
        public string StrAssistanceStaffID { get; set; }
        public int AssistanceID { get; set; }
        public int TimeTableSessions { get; set; }
        public string JobNumber { get; set; }
        public string Department { get; set; }
        public string DateOfJoining { get; set; }
        public string Qualification { get; set; }
        public string Specialization { get; set; }
        public string YearOfExperience { get; set; }
        public string YearofGraduation { get; set; }
        public string Institution { get; set; }
        public int ClassroomID { get; set; }
        public int SchoolClassID { get; set; }
        public int SchoolClassID2 { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public int SubjectID { get; set; }
        public string YearsOfExperience { get; set; }
        public string TotalYearsOfExperience { get; set; }
        public double Rate { get; set; }
        public int? Curriculum { get; set; }
        public int DepartmentID { get; set; }
        public int QualificationID { get; set; }
        public int SpecializationID { get; set; }
    }

    public class TeacherExperiences
    {
        public string SubjectName { get; set; }
        public string SchoolClassName { get; set; }
        public string AppraisalValueText { get; set; }
        public string YearsOfExperience { get; set; }
        public int TeacherExperienceID { get; set; }
        public int AppraisalValue { get; set; }
        public string SectionCode { get; set; }
        public int? NumberOfSessionsPerWeek { get; set; }
        public string TeacherID { get; set; }
    }

    public class TeacherSubjects
    {
        public string SubjectName { get; set; }
        public int SchoolClassID { get; set; }
        public string SchoolClassName { get; set; }
        public string SectionCode { get; set; }
        public string SectionName { get; set; }
        public int? NumberOfSessionsPerWeek { get; set; }
        public string TeacherID { get; set; }
    }

    public class StaffJobDetails
    {
        public string StaffID { get; set; }
        public string Institution { get; set; }
        public int? TotalYearsOfExperience { get; set; }
        public int YearofGraduation { get; set; }
        public DateTime DateOfJoining { get; set; }
        public int SpecializationID { get; set; }
        public int QualificationID { get; set; }
        public int DepartmentID { get; set; }
    }

    public class TeacherContactInfo
    {

        public string StaffID { get; set; }
        public string MobileNo { get; set; }
        public string Email { get; set; }
        public int NumberofSessins { get; set; }
        public int TimeTableSessions { get; set; }
        public int ClassroomID { get; set; }
        public string AssistanceStaffID { get; set; }

    }
}