using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartSchool.Models.Attendance
{
    public class ReviewAutoAttendanceVM
    {
        public string staffID { get; set; }
        public string tagIdentifier { get; set; }
        public string attendanceDate { get; set; }
        public string timeIn { get; set; }
        public string timeOut { get; set; }
        public string isPresence { get; set; }
        public string registerAttendanceMethod { get; set; }
    }
}