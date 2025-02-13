using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SmartSchool.Models.HomeWork
{
    public class HomeWork
    {
        public int HomeWorkID { set; get; }
        public int SchoolClassID { set; get; }
        public string SchoolClassName { set; get; }
        public List<SelectListItem> SchoolClassesList { set; get; }
        public int? SectionID { set; get; }
        public string SectionName { set; get; }
        public int? SubjectID { set; get; }
        public string SubjectName { set; get; }
        public string HomeWorkTitle { set; get; }
        public DateTime HomeWorkCreationDate { set; get; }
        public DateTime HomeWorkDeadLine { set; get; }
        public string HomeWorkAttachment { set; get; }
        public string Ext { set; get; }
        public string HomeWorkNote { set; get; }
        public string TeacherID { set; get; }
    }
}