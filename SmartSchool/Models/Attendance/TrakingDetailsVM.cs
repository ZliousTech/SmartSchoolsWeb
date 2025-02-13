using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartSchool.Models.Attendance
{
    public class TrakingDetailsVM
    {
        public string buildingNameEn { get; set; }
        public string buildingNameNative { get; set; }
        public string floorNameEn { get; set; }
        public string floorNameNative { get; set; }
        public string roomNameEn { get; set; }
        public string roomNameNative { get; set; }
        public string antennaNameEn { get; set; }
        public string antennaNameNative { get; set; }
        public string antennaZone { get; set; }
        public string tagDate { get; set; }
        public string tagTime { get; set; }
        public string tagTime12 { get; set; }
    }
}