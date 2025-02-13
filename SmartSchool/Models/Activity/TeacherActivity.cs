using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SmartSchool.Models.Activity
{
    public class TeacherActivity
    {
        public int ID { get; set; }
        public int SchoolID { get; set; }
        public string SchoolYear { get; set; }
        public int SchoolClassID { get; set; }
        public string SchoolClassName { get; set; }
        public List<SelectListItem> SchoolClassesList { get; set; }
        public int SectionID { get; set; }
        public string SectionName { get; set; }
        public byte[] Photo { get; set; }
        public string ArabicHeader { get; set; }
        public string EnglishHeader { get; set; }
        public string Header { get; set; }
        public string ArabicDescription { get; set; }
        public string EnglishDescription { get; set; }
        public int OccasionType { get; set; }
        public List<SelectListItem> OccasionTypeNames { get; set; }
        public DateTime StartingDate { get; set; }
        public int NumberofDays { get; set; }
        public bool Vacation { get; set; }
        public string TeacherID { get; set; }
    }

    public class StudentsActivity
    {
        public string StudentID { get; set; }
        public string StudentName { get; set; }
        public int SectionID { get; set; }
        public string SectionName { get; set; }

    }
}