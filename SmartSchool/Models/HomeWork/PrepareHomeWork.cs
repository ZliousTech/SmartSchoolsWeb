using System.Collections.Generic;
using System.Web.Mvc;

namespace SmartSchool.Models.HomeWork
{
    public class PrepareHomeWork
    {
        public List<HomeWork> HomeWork { get; set; }
        public int SchoolClassID { set; get; }
        public List<SelectListItem> SchoolClassesList { set; get; }
        public int? SectionID { set; get; }
        public List<SelectListItem> SubjectsList { set; get; }
        public int? SubjectID { set; get; }
        public string TeacherID { set; get; }
    }
}