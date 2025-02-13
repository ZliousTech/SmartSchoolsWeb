using Common.Helpers;
using System.Collections.Generic;

namespace SmartSchool.Models.VirtualClassRoom
{
    public class VirtualClassRoom
    {
        public int SchoolClassID { get; set; }

        public List<LookupDTO> Classes { get; set; }

        public int SectionID { get; set; }
        public int SubjectID { get; set; }

        //MSTeams_Account
        public string MsClientID { get; set; }
        public string MsTenantID { get; set; }

        public string hfSessionID { get; set; }

        public string hfAccessToken { get; set; }


        public string MeetingJoinURL { get; set; }

        public string MeetingTitle { get; set; }
    }
    public class RecipientStudent
    {
        public string StudentID { get; set; }
        public string StudentName { get; set; }


    }

}