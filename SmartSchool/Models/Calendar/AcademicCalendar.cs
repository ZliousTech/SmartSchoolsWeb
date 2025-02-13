using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace SmartSchool.Models.Calendar
{
    public class AcademicCalendar
    {
        public int OccasionID { get; set; }
        public byte[] Photo { get; set; }
        public string ArabicHeader { get; set; }
        public string EnglishHeader { get; set; }
        public string ArabicDescription { get; set; }
        public string EnglishDescription { get; set; }
        public int OccasionType { get; set; }
        public List<SelectListItem> OccasionTypeNames { get; set; }
        public DateTime StartingDate { get; set; }
        public int? NumberofDays { get; set; }
        public bool Vacation { get; set; }
        public int? CalendarID { get; set; }
        public string SchoolYear1 { get; set; }
    }
}