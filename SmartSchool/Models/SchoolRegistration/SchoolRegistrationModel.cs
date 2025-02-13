using Common.Helpers;
using Objects;
using System.Collections.Generic;

namespace SmartSchool.Models.SchoolRegistration
{
    public class SchoolRegistrationModel
    {

        public string CompanyName { get; set; }
        public string SchoolEmail { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Confirm { get; set; }



        public int DiscountTypeID { get; set; }


        public SchoolSetting SchoolSettings { get; set; }
        public SchoolBranch schoolBranch { get; set; }
        public List<double> BookFees { get; set; }
        public List<double> SchoolFees { get; set; }

        public List<double> UniformFees { get; set; }

        public List<SchoolBranch> SchoolBranchList { get; set; }
        public List<Department> AcademicDepartments { get; set; }
        public List<Department> Departments { get; set; }
        public List<Curriculum> Curriculum { get; set; }
        public List<Class> Classes { get; set; }
        public List<SchoolClass> SchoolClasses { get; set; }
        public List<TransportCategoryType> TransportCategoryTypes { get; set; }
        public List<Discount> Discounts { get; set; }

        public List<LookupDTO> Countries { get; set; }
        public List<LookupDTO> DiscountTypes { get; set; }

        public bool FeeYes { get; set; }
        public bool BookYes { get; set; }
        public bool UniformYes { get; set; }

        public bool AllSchoolsRegistered { get; set; }
        public bool OneSchoolsRegistered { get; set; }
        public byte[] Photo { get; set; }

    }
}