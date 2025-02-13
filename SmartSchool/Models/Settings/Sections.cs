using System.Collections.Generic;
using System.Web.Mvc;

namespace SmartSchool.Models.Settings
{
    public class Sections
    {
        public int SectionID { get; set; }
        public int SchoolID { get; set; }
        public int SchoolClassID { get; set; }
        public int ClassroomID { get; set; }
        public string SectionCode { get; set; }
        public int MaxNumberofStudents { get; set; }
        public int NumberofStudents { get; set; }
        public int IsFull { get; set; }
        public string SectionArabicName { get; set; }
        public string SectionEnglishName { get; set; }
        public string ClassroomDescription { get; set; }
        public string SchoolClassArabicName { get; set; }
        public string SchoolClassEnglishName { get; set; }
        public string RoomArabicName { get; set; }
        public string RoomEnglishName { get; set; }
        public string SectionName { get; set; }

        public List<SelectListItem> SchoolClassTextName { get; set; }
        public List<SelectListItem> RoomTextName { get; set; }
    }
}