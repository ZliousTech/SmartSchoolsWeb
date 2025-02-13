using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SmartSchool.Models.GoogleMapsDistanceAPI
{
    public class GoogleMapsDistanceAPI
    {
        public double SChoolLatitude { get; set; }
        public double SchoolLongitude { get; set; }
        public double StudentLatitude { get; set; }
        public double StudentLongitude { get; set; }
        public string DistanceinM { get; set; }
        public string DistanceinKM { get; set; }
    }
}