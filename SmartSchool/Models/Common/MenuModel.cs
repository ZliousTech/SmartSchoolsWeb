using Common.Helpers;
using System.Collections.Generic;

namespace SmartSchool.Models.Common
{
    public class MenuModel
    {
        public int CompanyID { get; set; }
        public List<LookupDTO> Companies { get; set; }
        public bool TimeTableTeacher { get; set; } = true;
        public bool TimeTableStudent { get; set; } = true;
        public bool Employees { get; set; } = true;
        public bool Teachers { get; set; } = true;
        public bool Students { get; set; } = true;
        public bool TimeAttendees { get; set; } = true;
        public bool Pay { get; set; } = true;
        public bool Map { get; set; } = true;
        public bool Calendar { get; set; } = true;
        public bool TimeTable { get; set; } = true;
        public bool SystemManagment { get; set; } = true;
        public bool Dashboard { get; set; } = true;



    }
}