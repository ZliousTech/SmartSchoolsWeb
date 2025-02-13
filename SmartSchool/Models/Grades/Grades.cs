namespace SmartSchool.Models.Grades
{
    public class Grades
    {
        public int? ID { get; set; }
        public int SchoolID { get; set; }
        public string SchoolYear { get; set; }
        public int ExamID { get; set; }
        public string ExamName { get; set; }
        public string StudentID { get; set; }
        public string StudentName { get; set; }
        public decimal GradeValue { get; set; }
        public decimal ExamMaxGrade { get; set; }
    }
}