using Common.Helpers;
using System.Collections.Generic;

namespace SmartSchool.Models.Student
{
    public class StudentModel
    {
        public int SchoolClassID { get; set; }

        public List<LookupDTO> Classes { get; set; }

        public int SectionID { get; set; }

        public List<Objects.Student> Students { get; set; }

        public int PendingRequests { get; set; }
    }
}