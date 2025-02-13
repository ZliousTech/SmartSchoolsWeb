using System.Collections.Generic;
using System.Web.Mvc;

namespace SmartSchool.Models.Settings
{
    public class Subjects
    {
        public int SubjectID { get; set; }
        public int SchoolID { set; get; }
        public int SchoolClassID { set; get; }
        public string SubjectArabicName { set; get; }
        public string SubjectEnglishName { set; get; }
        public int? MaxMark { set; get; }
        public int? FailMark { set; get; }
        public int? NumberOfSessionsPerWeek { set; get; }
        public int SchedulingCondition { set; get; }
        public int SectionID { set; get; }
        public bool IsOptional { set; get; }
        public string SchoolClassName { set; get; }
        public string SectionName { set; get; }
        public List<SelectListItem> SchoolClassTextName { get; set; }
        public List<SelectListItem> SectionTextName { get; set; }
        public List<SelectListItem> SchedulingConditionTextName { get; set; }
        public List<SelectListItem> IsOptionalTextName { get; set; }
    }
}