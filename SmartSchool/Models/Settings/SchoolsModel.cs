namespace SmartSchool.Models.Settings
{
    public class Viewheadquarters
    {
        public int CompanyID { get; set; }
        public string CompanyArabicName { get; set; }
        public string CompanyEnglishName { get; set; }
        public string ContactNo { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
    }

    public class Viewschoolbranches
    {
        public int SchoolID { get; set; }
        public string SchoolArabicName { get; set; }
        public string SchoolEnglishName { get; set; }
        public string SchoolContactNumber { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Email { get; set; }
    }
}