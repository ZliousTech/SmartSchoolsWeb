using System;
using System.Collections.Generic;

namespace Objects.DTO
{
    public class transportationDTO
    {
        public int schoolID { get; set; }
        public int BusID { get; set; }
        public string BusNo { get; set; }
        public string PlateNumber { get; set; }
        public string Driver { get; set; }
        public string Attendant { get; set; }
        public string SecondAttendant { get; set; }
        public int NumberOfseats { get; set; }
        public int? ParkingArea { get; set; }
        public DateTime BusLicenseDate { get; set; }
        public string AttendantID { get; set; }
        public string SecondAttendantID { get; set; }
        public int TourID { get; set; }
        public string TourName { get; set; }
        public string TourNameEn { get; set; }
        public string TourNameAr { get; set; }
        public string TourAreaName { get; set; }
        public int MaxNoofStudents { get; set; }
        public int NumberofStudents { get; set; }
        public int EstimatedTourTime { get; set; }
        public int EstimatedFuelConsumption { get; set; }
        public int TourDirection { get; set; }
        public DateTime StratingTime { get; set; }
        public double StraightDistanceFromSchool { get; set; }
        public double BestDistanceFromSchool { get; set; }

    }

    public class StudentDto
    {
        public string StudentID { get; set; }
        public string StudentName { get; set; }
        public string NationalNumber { get; set; }
        public int TransportCategoryTypeID { get; set; }
        public int AddressID { get; set; }
        public bool OnewayRetrun { get; set; }
        public bool OnewayGo { get; set; }
        public bool TwoWay { get; set; }
        public string TransportationDirection { get; set; }

    }
    public class TourDto
    {
        public string BusNo { get; set; }
        public int TourID { get; set; }
        public string TourName { get; set; }
        public string AreaName { get; set; }
        public int TourDirection { get; set; }

        public int MaxNumberOfStudents { get; set; }

        public int StudentsinTourCount { get; set; }

        public List<ScheduledBusTrip> ScheduledBusTrip { get; set; }

        public List<TourWayPoint> TourWayPoints { get; set; }
    }

    public class ScheduledBus
    {
        public string PassengerID { get; set; }
        public string PassengerName { get; set; }
        public long TourID { get; set; }
        public int TransportCategoryType { get; set; }
        public string TransportCategoryName { get; set; }

        public double StudentLat { get; set; }
        public double StudentLng { get; set; }
    }

    public class TourWayPoint
    {
        public double SchoolLat { get; set; }
        public double SchoolLng { get; set; }

        public double AttendantLat { get; set; }
        public double AttendantLng { get; set; }

        public double PassengerLat { get; set; }
        public double PassengerLng { get; set; }
    }

}
