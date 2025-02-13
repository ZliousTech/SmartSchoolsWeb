using SmartSchool.Models.TeacherStudentAttendance;
using System.Collections.Generic;

namespace SmartSchool.Models.Attendance
{
    public class PrepareAttendanceView
    {
        public PrepareAttendanceView()
        {
            Attendance = new Attendances();
            Students = new List<Objects.Student>();
            StudentAttendances = new List<TeacherStudentAttendances>();
        }
        public Attendances Attendance { get; set; }
        public List<Objects.Student> Students { get; set; }
        public List<TeacherStudentAttendances> StudentAttendances { get; set; }

        // TeacherAttendance Attribute.
        public string StaffID { get; set; }
        public int? WeekDay { get; set; }
        public int SessionDayOrder { get; set; }
    }
}