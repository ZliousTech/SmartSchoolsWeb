using Common.Helpers;
using System.Collections.Generic;

namespace SmartSchool.Models.SMS
{
    public class SmsViewModel
    {
        public int SchoolClassID { get; set; }
        public int TemplateID { get; set; }
        public List<LookupDTO> SchoolClassList { get; set; }
        public List<LookupDTO> MailTemplateList { get; set; }

    }

    public class Recipient
    {
        public int GuardianID { get; set; }
        public string GuardianName { get; set; }
        public string MobileNumber { get; set; }
    }
}