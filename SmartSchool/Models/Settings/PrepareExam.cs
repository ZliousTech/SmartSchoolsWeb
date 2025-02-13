using System.Collections.Generic;
using System.Web.Mvc;

namespace SmartSchool.Models.Settings
{
    public class PrepareExam
    {
        public List<Exam> Exams { get; set; }
        public int? SchoolClassID { get; set; }
        public List<SelectListItem> SchoolClassesList { get; set; }
        public int? SectionID { get; set; }
        public int? SubjectID { get; set; }
        public int? SemesterID { get; set; }
        public List<SelectListItem> SemestersList { get; set; }
        public int? ExamTypeID { get; set; }
        public List<SelectListItem> ExamTypesList { get; set; }
        public int? ExamTitleID { get; set; }
        public List<SelectListItem> ExamTitelsList { get; set; }
        public string TeacherID { get; set; }
    }
}