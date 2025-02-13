using Common.Helpers;
using Objects.DTO;
using SmartSchool.Models.Common;
using System.Collections.Generic;

namespace SmartSchool.Models.Home
{
    public class Dashboardstatisticsmodel
    {
        public int SchoolID { get; set; }
        public int CompanySchoolID { get; set; }
        public List<LookupDTO> SchoolList { get; set; }

        public int PrevSchoolID { get; set; }
        public string StaffID { get; set; }
        public int StaffType { get; set; }
        public string AcademicYear { get; set; }

        public MenuModel menumodel { get; set; }
        //Employees and teachers
        public int NumOfEmployees { get; set; }
        public int NumOfTeachers { get; set; }
        public int NumOfAdministrative { get; set; }
        public int NumOfBusEscort { get; set; }
        public int NumOfCleanWorker { get; set; }
        public int NumOfAccountants { get; set; }
        public int NumOfExperinces { get; set; }
        public int NumOfFirstTeacher { get; set; }
        public int NumOfSupervisorTeacher { get; set; }

        //Students
        public string StudentID { get; set; }
        public int? StudentSectionID { get; set; } = 0;
        public int NumOfYoungerStudents { get; set; }
        public int NumOfElderStudents { get; set; }
        public int NumOfStudents { get; set; }

        //Attendance
        public int NumOfFullAttendance { get; set; }
        public int NumOfPartialAttendance { get; set; }
        public int NumOfAbsent { get; set; }

        //Bus
        public int NumberOfBuses { get; set; }


        //menu
        public int CompanyID { get; set; }
        public List<LookupDTO> Companies { get; set; }
        public bool TimeTableTeacher { get; set; } = true;
        public bool TimeTableStudent { get; set; } = true;
        public bool Employees { get; set; } = true;
        public bool Teachers { get; set; } = true;
        public bool Students { get; set; } = true;
        public bool RegisterRequests { get; set; } = true;
        public bool Buses { get; set; } = true;
        public bool TimeAttendees { get; set; } = true;
        public bool Pay { get; set; } = true;
        public bool Map { get; set; } = true;
        public bool Calendar { get; set; } = true;
        public bool TimeTable { get; set; } = true;
        public bool SystemManagment { get; set; } = true;
        public bool SchoolDashboard { get; set; } = true;
        public bool QuestionsBank { get; set; } = true;

        public bool VirtualClassRoom { get; set; } = false;
        public bool SendSms { get; set; } = false;
        public bool SystemUsers { get; set; } = false;
        public bool StudentRegistration { get; set; } = false;

        public bool GenerateUsers { get; set; } = false;

        //////public int PendingRequests { get; set; }

        //External Guardian 
        public bool ExternalGuardianDashboard { get; set; } = false;
        public bool ShowNotification { get; set; } = true;
        public bool ShowQeustionsForms { get; set; } = true;

        public IEnumerable<NotificationDTO> NotifcationWeb { get; set; }
        public bool IsActive { get; set; }
        public byte[] Photo { get; set; }

    }
}