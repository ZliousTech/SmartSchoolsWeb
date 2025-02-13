using System.Collections.Generic;
using System.Web.Mvc;

namespace SmartSchool.Models.Calendar
{
    public class AcademicCalendars
    {
        public int CalendarID { get; set; }
        public string CountryID { get; set; }
        public List<SelectListItem> CountryNames { get; set; }
        public int? SchoolID { get; set; }
        public int? CompanyID { get; set; }
        public string SchoolYear { get; set; }
    }
}