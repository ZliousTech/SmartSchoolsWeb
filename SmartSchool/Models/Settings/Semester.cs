namespace SmartSchool.Models.Settings
{
    public class Semester
    {
        public int ID { get; set; }
        public string SemesterArabicName { get; set; }
        public string SemesterEnglishName { get; set; }
        public string SemesterName { get; set; }
        public int SchoolID { get; set; }
        public string SchoolYear { get; set; }
    }
}