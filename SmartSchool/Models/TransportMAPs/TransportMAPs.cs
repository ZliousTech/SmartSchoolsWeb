using Objects;
using System.Collections.Generic;
namespace SmartSchool.Models.TransportMAPs
{
    public class TransportMAPs
    {
        public double SChoolLatitude { get; set; }
        public double SchoolLongitude { get; set; }


        public double Distanceinmeters1 { get; set; }
        public double Distanceinkilometers1 { get; set; }
        public double Distanceinmeters2 { get; set; }
        public double Distanceinkilometers2 { get; set; }
        public double Distanceinmeters3 { get; set; }
        public double Distanceinkilometers3 { get; set; }
        public double Distanceinmeters4 { get; set; }
        public double Distanceinkilometers4 { get; set; }
        public double Distanceinmeters5 { get; set; }
        public double Distanceinkilometers5 { get; set; }


        public double GoCost_1 { get; set; }
        public double GoCost_2 { get; set; }
        public double GoCost_3 { get; set; }
        public double GoCost_4 { get; set; }
        public double GoCost_5 { get; set; }

        public int ReturnCost_1 { get; set; }
        public int ReturnCost_2 { get; set; }
        public int ReturnCost_3 { get; set; }
        public int ReturnCost_4 { get; set; }
        public int ReturnCost_5 { get; set; }

        public int TwoWay_1 { get; set; }
        public int TwoWay_2 { get; set; }
        public int TwoWay_3 { get; set; }
        public int TwoWay_4 { get; set; }
        public int TwoWay_5 { get; set; }

        public double StudentLatitude { get; set; }
        public double StudentLongitude { get; set; }
        public string DistanceinM { get; set; }
        public string DistanceinKM { get; set; }
        public int SchoolID { get; set; }
        public string Category { get; set; }
        public string SubscribeTransportation { get; set; }
        public List<TransportCategoryType> TransportCategoryTypes { get; set; }
    }
}