namespace SmartSchool.Models.TeacherStudentAttendance
{
    public class TeacherStudentAttendances
    {
        public string StudentID { get; set; }
        public string StudentName { get; set; }
        public string StudentEnglishName { get; set; }
        public string StudentClass { get; set; }
        public string StudentEnglisClass { get; set; }
        public int SectionID { get; set; }
        public string StudentSection { get; set; }
        public string StudentEnglisSection { get; set; }
        public int StudentTotalAbsence { get; set; }
        public int StudenTotalPartialAttendace { get; set; }

    }
}