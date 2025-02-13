using System.Collections.Generic;
using System.Web.Mvc;

namespace SmartSchool.Models.Settings
{
    public class Curriculums
    {
        public int CurriculumID { get; set; }
        public string CurriculumArabicName { get; set; }
        public string CurriculumEnglishName { get; set; }
        public string DepartmentArabicName { get; set; }
        public string DepartmentEnglishName { get; set; }

        public int? DepartmentID { get; set; }
        public List<SelectListItem> DepartmentTextName { get; set; }

    }
}