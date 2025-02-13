namespace Common
{
    public class StudentNotificationDTO
    {
        public int NotificationID { get; set; }
        public string TeacherID { get; set; }
        public int SchoolClassID { get; set; }
        public int SectionID { get; set; }
        public int SubjectID { get; set; }
        public string NotificationType { get; set; }
    }
}
