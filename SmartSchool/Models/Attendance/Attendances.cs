using System.Collections.Generic;
using System.Web.Mvc;

namespace SmartSchool.Models.Attendance
{
    public class Attendances
    {
        public int AttendanceItemID { set; get; }
        public int SchoolID { set; get; }
        public string SchoolYear { set; get; }
        public string StudentID { set; get; }
        public string AttendanceDate { set; get; }
        public int? AttendanceType { set; get; }
        public int AbsenceReason { set; get; }
        public string Description { set; get; }
        public bool ParentInformed { set; get; }
        public string ParentMessage { set; get; }
        public string AttendanceNote { set; get; }
        public bool FirstSession { set; get; }
        public bool SecondSession { set; get; }
        public bool ThirdSession { set; get; }
        public bool FourthSession { set; get; }
        public bool FifthSession { set; get; }
        public bool SixthSession { set; get; }
        public bool SeventhSession { set; get; }
        public bool EighthSession { set; get; }
        public string FirstSessionTimeStamp { set; get; }
        public string SecondSessionTimeStamp { set; get; }
        public string ThirdSessionTimeStamp { set; get; }
        public string FourthSessionTimeStamp { set; get; }
        public string FifthSessionTimeStamp { set; get; }
        public string SixthSessionTimeStamp { set; get; }
        public string SeventhSessionTimeStam { set; get; }
        public string EighthSessionTimeStamp { set; get; }
        public bool IsAbsence { set; get; }

        public List<SelectListItem> AttendanceTypeTextName { set; get; }
        public List<SelectListItem> AbsenceReasonTextName { set; get; }

        public int? SchoolClassID { set; get; }
        public List<SelectListItem> SchoolClassesList { set; get; }
        public int? SectionID { set; get; }

    }
}