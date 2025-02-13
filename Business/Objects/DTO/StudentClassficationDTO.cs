using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.DTO
{
   public class StudentClassficationDTO
    {
        public string StudentID { get; set; }
        public int AgeClassificationID { get; set; }
        public string AgeClassificationArabicText { get; set; }
        public string AgeClassificationEnglishText { get; set; }

    }
}
