namespace Common
{
    public class clsenumration
    {

        public enum UserData
        {
            UserID = 1,
            Username = 2,
            SchoolID = 3,
            UserType = 4,
            GuardianID = 5,
            StaffID = 6,
            CompanyID = 7,
            PreviousSchoolID = 8,
            StudentID = 9

        }

        public enum UserTypes
        {
            student = 8

        }

        public enum RequestStatus
        {
            NotSent,
            pending,
            Accepted,
            Rejected
        }
        public enum DepartmentType
        {
            Administrative = 1,
            Academic = 2
        }
        public enum FeeType
        {
            RegistrationFee = 1,
            Uniform = 4,
            Books = 5,

        }
        public enum MeetingUserGroups
        {
            Teacher = 1,
            Student = 2
        }
        public enum NotificationTypes
        {
            OnlineMeeting = 1
        }
        public enum NotificationsTypes
        {
            Meeting = 1,
            Homework = 2,
            Exam = 3,
            Attendance = 4,
            Bus = 5
        }
        public enum UsersGroups
        {
            Teacher = 1,
            Student = 2,
            Guardian = 3
        }
    }
}
