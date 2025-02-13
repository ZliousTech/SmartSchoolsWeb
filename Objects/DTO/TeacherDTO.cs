namespace Objects.DTO
{
    public class TeacherDTO
    {
        public string StaffID { get; set; }
        public string TeacherName { get; set; }
        public int? NumberofSessins { get; set; }
        public int? TimeTableSessions { get; set; }
        public string HexColor { get; set; }
        public int? SectionID { get; set; }
        public string SectionCode { get; set; }
    }

    public class TeacherTimeTable
    {
        public string TeacherID { get; set; }
        public string SubjectArabicName { get; set; }
        public string SubjectEnglishName { get; set; }
        public string ItemRGBColor { get; set; }
        public string SchoolClassArabicName { get; set; }
        public string SchoolClassEnglishName { get; set; }
        public string SectionCode { get; set; }
        public string SectionArabicName { get; set; }
        public string SectionEnglishName { get; set; }
        public string StaffArabicName { get; set; }
        public int SessionDayOrder { get; set; }
    }

}
