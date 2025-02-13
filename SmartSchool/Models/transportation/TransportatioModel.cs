using Common.Helpers;
using Objects.DTO;
using System.Collections.Generic;

namespace SmartSchool.Models.transportation
{
    public class TransportatioModel
    {
        //BUS
        public int schoolID { get; set; }
        public int BusID { get; set; }
        public string BusNo { get; set; }
        public string PlateNumber { get; set; }
        public string DriverID { get; set; }
        public string AttendantID { get; set; }
        public string SecondAttendantID { get; set; }
        public string Attendant { get; set; }
        public string SecondAttendant { get; set; }
        public int NumberOfseats { get; set; }
        public int? ParkingArea { get; set; }
        public string BusLicenseDate { get; set; }
        public List<LookupDTO> schoolList { get; set; }
        public List<LookupDTO> DriverList { get; set; }
        public List<LookupDTO> EscortList { get; set; }

        //TOUR 

        public int TourID { get; set; }
        public int AreaID { get; set; }
        public string TourNameEn { get; set; }
        public string TourNameAr { get; set; }
        public string TourAreaName { get; set; }
        public int MaxNoofStudents { get; set; }
        public int NumberofStudents { get; set; }
        public int EstimatedTourTime { get; set; }
        public int EstimatedFuelConsumption { get; set; }
        public int TourDirection { get; set; }
        public string StratingTime { get; set; }
        public double StraightDistanceFromSchool { get; set; }
        public double BestDistanceFromSchool { get; set; }
        public int StudentsinTourCount { get; set; }
        public List<LookupDTO> TourAreaList { get; set; }
        public List<LookupDTO> BusList { get; set; }
        public List<StudentDto> StudentList { get; set; }
        public List<LookupDTO> TourList { get; set; }

        //IDS For list 
        public int TourIDA { get; set; }
        public int TourIDB { get; set; }
        public int TourIDC { get; set; }
        public int TourIDD { get; set; }
        public int TourIDE { get; set; }

        public List<Students> Students { get; set; }

        public List<ScheduledBusTrips> ScheduledStudents { get; set; }

    }

    public class Students
    {
        public string StudentID { get; set; }
    }

    public class ScheduledBusTrips
    {
        public int TourID { get; set; }

        public int SchoolID { get; set; }

        public string LastDate { get; set; }

        public string StudentID { get; set; }

        public string StudentName { get; set; }

        public string StudentLat { get; set; }

        public string StudentLng { get; set; }
    }
}