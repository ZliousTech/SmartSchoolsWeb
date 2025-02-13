using System;

namespace SmartSchool.Models.System
{
    public class SystemSettingsModel
    {
        //SystemSettingsPerSchool
        public int SchoolID { get; set; }
        public Nullable<int> LastInvoiceNumber { get; set; }
        public Nullable<int> OptionalArabicFields { get; set; }
        public Nullable<int> OptionalEnglishFields { get; set; }
        public Nullable<int> OptionalFamilyDetails { get; set; }
        public Nullable<int> OptionalThirdName { get; set; }
        public Nullable<int> BusScheduleDateType { get; set; }
        public Nullable<int> BusScheduleType { get; set; }
        public Nullable<int> NumberofBusTrips { get; set; }
        public Nullable<int> TimetableType { get; set; }
        public Nullable<int> SubjectDistributionMethod { get; set; }
        public Nullable<int> SubjectforClassorSection { get; set; }
        public Nullable<int> CurrencyOptions { get; set; }
        public Nullable<int> BusAttendantAssigningMethod { get; set; }

        //SchoolSettings
        public Nullable<int> NumberofClassRooms { get; set; }
        public Nullable<int> NumberofChairsPerClass { get; set; }
        public Nullable<int> NumberofSessionsPerDay { get; set; }
        public Nullable<int> NumberofSessionsPerTeacher { get; set; }
        public Nullable<int> AllowedNumberofStudentsPerYardArea { get; set; }
        public Nullable<int> AllowedNumberofStudentsPerClassroomArea { get; set; }
        public Nullable<int> WeekStartingDay { get; set; }
        public string StartingTime { get; set; }
        public string FirstClassStartingTime { get; set; }
        public Nullable<int> SessionDuration { get; set; }
        public Nullable<int> BreakBetweenSessionsDuration { get; set; }
        public Nullable<int> BreakDuration { get; set; }
        public Nullable<int> NumberofSemesters { get; set; }
        public Nullable<int> NumberofExamsPerSemester { get; set; }
        public Nullable<int> MultiDependantsDiscount { get; set; }
        public Nullable<int> BreakAfterSession { get; set; }
        public Nullable<int> MaxDiscountNumber { get; set; }

    }
}