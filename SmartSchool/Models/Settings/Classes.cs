using System.Collections.Generic;
using System.Web.Mvc;

namespace SmartSchool.Models.Settings
{
    public class Classes
    {
        public int SchoolClassID { get; set; }
        public int ClassID { get; set; }
        public int SchoolID { get; set; }
        public int CurriculumID { get; set; }
        public string SchoolClassEnglishName { get; set; }
        public string SchoolClassArabicName { get; set; }
        public bool Coeducation { get; set; }
        public bool Male { get; set; }
        public bool Female { get; set; }
        public string ClassArabicName { get; set; }
        public string ClassEnglishName { get; set; }
        public string CurriculumArabicName { get; set; }
        public string CurriculumEnglishName { get; set; }

        public List<SelectListItem> ClassTextName { get; set; }
        public List<SelectListItem> CurriculumTextName { get; set; }
    }
}