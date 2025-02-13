namespace SmartSchool.Models.Calendar
{
    public class AcademicCalendarView
    {
        public AcademicCalendarView()
        {
            AcademicCalendars = new AcademicCalendars();
            AcademicCalendar = new AcademicCalendar();
        }

        public AcademicCalendars AcademicCalendars { get; set; }
        public AcademicCalendar AcademicCalendar { get; set; }
    }
}