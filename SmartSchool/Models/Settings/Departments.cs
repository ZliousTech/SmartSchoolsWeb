namespace SmartSchool.Models.Settings
{
    public class Departments
    {
        public Departments()
        {
            DepartmentID = 0;
            DepartmentArabicName = "";
            DepartmentEnglishName = "";
            DepartmentType = "";
        }

        public int DepartmentID { get; set; }
        public string DepartmentArabicName { get; set; }
        public string DepartmentEnglishName { get; set; }
        public string DepartmentType { get; set; }
    }
}