namespace Objects.DTO
{
    public class ClassTimeTableDTO
    {
        public string TeacherID { get; set; }
        public int TimetableEntityID { get; set; }
        public string StaffArabicName { get; set; }
        public string StaffEnglishName { get; set; }
        public int SubjectID { get; set; }
        public string SubjectArabicName { get; set; }
        public string SubjectEnglishName { get; set; }
        public int SessionDayOrder { get; set; }
    }
}
