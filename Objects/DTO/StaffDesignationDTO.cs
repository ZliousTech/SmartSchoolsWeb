using System;

namespace Objects.DTO
{
    public class StaffDesignationDTO
    {
        public int SchooolID { get; set; }
        public String StaffID { get; set; }
        public int DesignationID { get; set; }
        public int DepartmentID { get; set; }
        public string DesignationArabicText { get; set; }
        public string DesignationEnglishText { get; set; }

    }
}
