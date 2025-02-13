using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartSchool.Models.Attendance
{
    public class AttendanceTrackingVM
    {
        public string name { set; get; }
        public string staffID {set; get;}
        public string tagID {set; get;}
        public string buildingNameEn {set; get;}
        public string buildingNameNative {set; get;}
        public string floorNameEn {set; get;}
        public string floorNameNative {set; get;}
        public string roomNameEn {set; get;}
        public string roomNameNative {set; get;}
        public string antennaNameEn {set; get;}
        public string antennaNameNative {set; get;}
        public string antennaZone {set; get;}
        public string tagDate {set; get;}
        public string tagTime {set; get;}
        public string tagTime12 { set; get;}
    }
}