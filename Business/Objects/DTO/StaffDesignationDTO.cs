using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objects.DTO
{
  public  class StaffDesignationDTO
    {
        public int SchooolID { get; set; }
        public String StaffID { get; set; }
        public int DesignationID { get; set; }
        public int DepartmentID { get; set; }
        public string DesignationArabicText { get; set; }
        public string DesignationEnglishText { get; set; }

    }
}
