using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SmartSchool.Models.Settings
{
    public class Exam
    {
        public int ID { get; set; }
        public int SchoolID { get; set; }
        public string SchoolYear { get; set; }
        public int SchoolClassID { get; set; }
        public string SchoolClassName { get; set; }
        public string SchoolClassEnglishName { get; set; }
        public List<SelectListItem> SchoolClassesList { get; set; }
        public int SectionID { get; set; }
        public string SectionName { get; set; }
        public string SectionEnglisName { get; set; }
        public int SemesterID { get; set; }
        public string SemesterName { get; set; }
        public List<SelectListItem> SemestersList { get; set; }
        public int SubjectID { get; set; }
        public string SubjectName { get; set; }
        public string SubjectEnglisName { get; set; }
        public int ExamTypeID { get; set; }
        public string ExamTypeName { get; set; }
        public List<SelectListItem> ExamTypesList { get; set; }
        public int ExamTitleID { get; set; }
        public string ExamTitleName { get; set; }
        public List<SelectListItem> ExamTitelsList { get; set; }
        public decimal TotalGrades { get; set; }
        public string TeacherID { get; set; }
        public bool IsCounted { get; set; }
        public DateTime? ExamDate { get; set; }
    }
}