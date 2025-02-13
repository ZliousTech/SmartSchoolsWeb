using System.Collections.Generic;
using System.Web.Mvc;

namespace SmartSchool.Models.Activity
{
    public class PrepareTeacherActivity
    {
        public List<TeacherActivity> TeacherActivities { set; get; }
        public List<TeacherStudentsActivity> TeacherStudentActivities { set; get; }
        public int? SchoolClassID { get; set; }
        public List<SelectListItem> SchoolClassesList { get; set; }
        public int? SectionID { get; set; }
        public string TeacherID { get; set; }
    }
}