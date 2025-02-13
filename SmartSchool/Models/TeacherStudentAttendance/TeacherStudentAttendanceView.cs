using SmartSchool.Models.Attendance;
using System.Collections.Generic;

namespace SmartSchool.Models.TeacherStudentAttendance
{
    public class TeacherStudentAttendanceView
    {
        public TeacherStudentAttendanceView()
        {
            Attendance = new Attendances();
            TeacherStudentAttendance = new List<TeacherStudentAttendances>();
        }
        public Attendances Attendance { get; set; }
        public List<TeacherStudentAttendances> TeacherStudentAttendance { get; set; }
    }
}