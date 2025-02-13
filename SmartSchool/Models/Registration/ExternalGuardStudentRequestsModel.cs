using System;

namespace SmartSchool.Models.Registration
{
    public class ExternalGuardStudentRequestsModel
    {
        public string StudentID { get; set; }
        public string StudentName { get; set; }
        public int GuardianID { get; set; }
        public Nullable<int> RequestStatus { get; set; }
    }
}