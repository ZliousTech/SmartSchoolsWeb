using System.Collections.Generic;

namespace SmartSchool.Models.Registration
{
    public class ParentRegistrationModel
    {
        public int GuardianID { get; set; }
        public string ParentName { get; set; }
        public List<ExternalGuardStudentRequestsModel> StudentsRequests { get; set; }
    }
}